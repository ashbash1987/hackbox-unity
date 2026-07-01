#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hackbox
{
    // Standalone/editor transport for the hackbox relay.
    //
    // The relay is a Cloudflare Durable Object that speaks raw WebSocket, with
    // every frame a JSON envelope `{ type, payload }`. This hand-rolls a
    // ClientWebSocket connection and maps it back onto the same On/Emit surface
    // the bundled socket.io client used to expose, so Host.cs is unaffected: the
    // application protocol (event names + payloads) is unchanged - only the wire
    // moved from socket.io/engine.io to raw WebSocket.
    //
    //   host  -> relay : member.update { to, data }, reload
    //   relay -> host  : state.host { members }, msg { ... }, change { ... }
    internal class StandaloneSocket : ISocketIO
    {
        internal StandaloneSocket(string url)
        {
            _url = new Uri(url);
        }

        // Keepalive: idle sockets get dropped by consumer-router/CGNAT timeouts, so
        // we send a "ping" text frame on this interval; the relay answers "pong" at
        // the edge. Must match the relay's setWebSocketAutoResponse string.
        private const string KeepalivePing = "ping";
        private const string KeepalivePong = "pong";
        private static readonly TimeSpan PingInterval = TimeSpan.FromSeconds(25.0);

        // Reconnect backoff, mirroring the player client's partysocket settings.
        private static readonly TimeSpan MinReconnectDelay = TimeSpan.FromMilliseconds(250.0);
        private static readonly TimeSpan MaxReconnectDelay = TimeSpan.FromSeconds(10.0);
        private const double ReconnectDelayGrowFactor = 2.0;

        // Close codes >= 4000 are deliberate relay rejections (room gone/closed,
        // Twitch required, duplicate device, expired). They must not trigger a
        // reconnect; the relay sends a human-readable `error` frame first, then
        // closes with the reason.
        private const int FatalCloseThreshold = 4000;

        private readonly Uri _url;
        private readonly Dictionary<string, Action<JObject>> _handlers = new Dictionary<string, Action<JObject>>();
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);

        private ClientWebSocket _ws = null;
        private CancellationTokenSource _cts = null;
        private bool _manualClose = false;
        private string _lastErrorMessage = null;
        private DateTime _lastPingSentUtc;

        public event Action OnConnected;
        public event Action<string> OnError;
        public event Action<string> OnDisconnected;
        public event Action<int> OnReconnectAttempt;
        public event Action<int> OnReconnected;
        public event Action OnReconnectFailed;
        public event Action OnPing;
        public event Action<TimeSpan> OnPong;

        public bool Connected => _ws != null && _ws.State == WebSocketState.Open;
        public bool Disconnected => !Connected;

        public async Task Connect()
        {
            _manualClose = false;
            _cts = new CancellationTokenSource();

            try
            {
                await OpenSocket(_cts.Token);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
                return;
            }

            OnConnected?.Invoke();
            _ = Task.Run(() => MaintainConnection(_cts.Token));
        }

        public async Task Disconnect()
        {
            _manualClose = true;
            _cts?.Cancel();

            if (_ws != null)
            {
                try
                {
                    if (_ws.State == WebSocketState.Open)
                    {
                        await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "client disconnect", CancellationToken.None);
                    }
                }
                catch (Exception)
                {
                    // Best-effort close; the socket is going away regardless.
                }

                _ws.Dispose();
                _ws = null;
            }
        }

        public async Task Emit(string eventName, string message)
        {
            // Wrap the (already-serialised) payload in the relay's envelope. Parsing
            // `message` back to a JToken embeds it as raw JSON rather than a string.
            JObject envelope = new JObject { ["type"] = eventName };
            if (!string.IsNullOrEmpty(message))
            {
                envelope["payload"] = JToken.Parse(message);
            }

            await SendText(envelope.ToString(Formatting.None));
        }

        public void On(string eventName, Action<JObject> messageHandler)
        {
            _handlers[eventName] = messageHandler;
        }

        public void Off(string eventName)
        {
            _handlers.Remove(eventName);
        }

        // Run the receive + keepalive loops for the current connection, reconnecting
        // with backoff on a transient drop and stopping on a fatal close or a manual
        // disconnect.
        private async Task MaintainConnection(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bool terminal;
                using (CancellationTokenSource connectionCts = CancellationTokenSource.CreateLinkedTokenSource(token))
                {
                    Task ping = PingLoop(connectionCts.Token);
                    terminal = await ReceiveLoop(connectionCts.Token);
                    connectionCts.Cancel();
                    await SafeAwait(ping);
                }

                if (terminal || _manualClose || token.IsCancellationRequested)
                {
                    return;
                }

                if (!await Reconnect(token))
                {
                    return;
                }
            }
        }

        // Drain the socket until it closes or drops. Returns true when the outcome is
        // terminal (fatal close or manual cancellation) and the caller should stop,
        // false for a transient drop that should be reconnected.
        private async Task<bool> ReceiveLoop(CancellationToken token)
        {
            byte[] buffer = new byte[8192];
            StringBuilder builder = new StringBuilder();

            try
            {
                while (!token.IsCancellationRequested)
                {
                    WebSocketReceiveResult result;
                    builder.Clear();
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            return HandleClose();
                        }

                        builder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                    }
                    while (!result.EndOfMessage);

                    HandleMessage(builder.ToString());
                }
            }
            catch (OperationCanceledException)
            {
                // Cancellation means a deliberate disconnect; don't reconnect.
                return true;
            }
            catch (Exception)
            {
                // Transport-level failure: reconnect unless we were closing on purpose.
                return _manualClose;
            }

            return _manualClose;
        }

        private bool HandleClose()
        {
            int code = _ws.CloseStatus.HasValue ? (int)_ws.CloseStatus.Value : 0;
            string reason = !string.IsNullOrEmpty(_lastErrorMessage)
                ? _lastErrorMessage
                : (!string.IsNullOrEmpty(_ws.CloseStatusDescription) ? _ws.CloseStatusDescription : "transport close");

            if (code >= FatalCloseThreshold)
            {
                OnError?.Invoke(reason);
                OnDisconnected?.Invoke(reason);
                return true;
            }

            // Transient close - reconnect (unless this was a manual disconnect).
            return _manualClose;
        }

        private void HandleMessage(string text)
        {
            // The relay auto-responds to our keepalive; surface it as a pong.
            if (text == KeepalivePong)
            {
                OnPong?.Invoke(DateTime.UtcNow - _lastPingSentUtc);
                return;
            }

            JObject envelope;
            try
            {
                envelope = JObject.Parse(text);
            }
            catch (Exception)
            {
                return;
            }

            string type = (string)envelope["type"];
            if (string.IsNullOrEmpty(type))
            {
                return;
            }

            JObject payload = envelope["payload"] as JObject;

            // Stash the relay's application-level error message so the fatal close
            // that follows can surface it through OnError/OnDisconnected.
            if (type == "error")
            {
                _lastErrorMessage = payload != null ? (string)payload["message"] : null;
            }

            if (_handlers.TryGetValue(type, out Action<JObject> handler))
            {
                handler?.Invoke(payload);
            }
        }

        private async Task PingLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(PingInterval, token);

                    if (_ws == null || _ws.State != WebSocketState.Open)
                    {
                        return;
                    }

                    _lastPingSentUtc = DateTime.UtcNow;
                    OnPing?.Invoke();
                    await SendText(KeepalivePing);
                }
            }
            catch (OperationCanceledException)
            {
                // Connection closed; nothing to do.
            }
            catch (Exception)
            {
                // A failed ping just means the socket is dropping - the receive loop
                // will observe and handle it.
            }
        }

        private async Task<bool> Reconnect(CancellationToken token)
        {
            int attempt = 0;
            while (!token.IsCancellationRequested && !_manualClose)
            {
                attempt++;
                OnReconnectAttempt?.Invoke(attempt);

                try
                {
                    await Task.Delay(BackoffDelay(attempt), token);
                }
                catch (OperationCanceledException)
                {
                    return false;
                }

                try
                {
                    await OpenSocket(token);
                    OnReconnected?.Invoke(attempt);
                    return true;
                }
                catch (Exception)
                {
                    // Keep retrying with a growing delay (mirrors partysocket).
                }
            }

            return false;
        }

        private async Task OpenSocket(CancellationToken token)
        {
            if (_ws != null)
            {
                try
                {
                    _ws.Dispose();
                }
                catch (Exception)
                {
                    // Disposing a faulted socket can throw; safe to ignore.
                }
            }

            _lastErrorMessage = null;
            _ws = new ClientWebSocket();
            await _ws.ConnectAsync(_url, token);
        }

        private async Task SendText(string text)
        {
            if (_ws == null || _ws.State != WebSocketState.Open)
            {
                return;
            }

            CancellationToken token = _cts != null ? _cts.Token : CancellationToken.None;
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            await _sendLock.WaitAsync(token);
            try
            {
                await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, token);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private static TimeSpan BackoffDelay(int attempt)
        {
            double ms = MinReconnectDelay.TotalMilliseconds * Math.Pow(ReconnectDelayGrowFactor, attempt - 1);
            return TimeSpan.FromMilliseconds(Math.Min(ms, MaxReconnectDelay.TotalMilliseconds));
        }

        private static async Task SafeAwait(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception)
            {
                // The loop's own error handling already accounts for failures.
            }
        }
    }
}
#endif

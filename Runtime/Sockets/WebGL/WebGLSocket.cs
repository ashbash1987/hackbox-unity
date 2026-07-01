#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AOT;

namespace Hackbox
{
    // WebGL transport for the hackbox relay.
    //
    // `System.Net.WebSockets.ClientWebSocket` does not work under WebGL, so the
    // browser `WebSocket` is driven from the bundled `WebGLSocket.jslib` (which
    // also owns the keepalive ping and the reconnect backoff, since WebGL has no
    // background threads). This class is the managed side: it forwards lifecycle
    // callbacks as ISocketIO events and dispatches each `{ type, payload }` frame
    // to the registered handler - the same On/Emit surface as the standalone
    // transport, so Host.cs is unaffected.
    internal class WebGLSocket : ISocketIO
    {
        // [MonoPInvokeCallback] callbacks must be static, so the jslib's callbacks
        // fan out to every live instance (Host only ever holds one).
        private static readonly List<WebGLSocket> Instances = new List<WebGLSocket>();

        private readonly Dictionary<string, Action<JObject>> _handlers = new Dictionary<string, Action<JObject>>();
        private string _lastErrorMessage = null;

        internal WebGLSocket(string url)
        {
            Instances.Add(this);
            WebSocketInit(url,
                          DelegateOnConnect,
                          DelegateOnError,
                          DelegateOnDisconnect,
                          DelegateOnReconnectAttempt,
                          DelegateOnReconnect,
                          DelegateOnPing,
                          DelegateOnPong,
                          DelegateOnMessage);
        }

        ~WebGLSocket()
        {
            Instances.Remove(this);
        }

        public event Action OnConnected;
        public event Action<string> OnError;
        public event Action<string> OnDisconnected;
        public event Action<int> OnReconnectAttempt;
        public event Action<int> OnReconnected;
        public event Action OnReconnectFailed;
        public event Action OnPing;
        public event Action<TimeSpan> OnPong;

        public bool Connected => WebSocketConnected();
        public bool Disconnected => WebSocketDisconnected();

        public Task Connect()
        {
            WebSocketConnect();
            return Task.CompletedTask;
        }

        public Task Disconnect()
        {
            WebSocketDisconnect();
            return Task.CompletedTask;
        }

        public Task Emit(string eventName, string message)
        {
            // Wrap the (already-serialised) payload in the relay's envelope. Parsing
            // `message` back to a JToken embeds it as raw JSON rather than a string.
            JObject envelope = new JObject { ["type"] = eventName };
            if (!string.IsNullOrEmpty(message))
            {
                envelope["payload"] = JToken.Parse(message);
            }

            WebSocketSend(envelope.ToString(Formatting.None));
            return Task.CompletedTask;
        }

        public void On(string eventName, Action<JObject> messageHandler)
        {
            _handlers[eventName] = messageHandler;
        }

        public void Off(string eventName)
        {
            _handlers.Remove(eventName);
        }

        private void Dispatch(string text)
        {
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

        [MonoPInvokeCallback(typeof(VoidCallback))]
        private static void DelegateOnConnect()
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.OnConnected?.Invoke();
            }
        }

        [MonoPInvokeCallback(typeof(StringCallback))]
        private static void DelegateOnError(string error)
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.OnError?.Invoke(error);
            }
        }

        [MonoPInvokeCallback(typeof(StringCallback))]
        private static void DelegateOnDisconnect(string reason)
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.OnDisconnected?.Invoke(reason);
            }
        }

        [MonoPInvokeCallback(typeof(IntCallback))]
        private static void DelegateOnReconnectAttempt(int attempt)
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.OnReconnectAttempt?.Invoke(attempt);
            }
        }

        [MonoPInvokeCallback(typeof(IntCallback))]
        private static void DelegateOnReconnect(int attempt)
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.OnReconnected?.Invoke(attempt);
            }
        }

        [MonoPInvokeCallback(typeof(VoidCallback))]
        private static void DelegateOnPing()
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.OnPing?.Invoke();
            }
        }

        [MonoPInvokeCallback(typeof(VoidCallback))]
        private static void DelegateOnPong()
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.OnPong?.Invoke(TimeSpan.Zero);
            }
        }

        [MonoPInvokeCallback(typeof(StringCallback))]
        private static void DelegateOnMessage(string text)
        {
            foreach (WebGLSocket instance in Instances)
            {
                instance.Dispatch(text);
            }
        }

        private delegate void VoidCallback();
        private delegate void StringCallback(string value);
        private delegate void IntCallback(int value);

        [DllImport("__Internal")]
        private static extern void WebSocketInit(string url, VoidCallback onConnect, StringCallback onError, StringCallback onDisconnect, IntCallback onReconnectAttempt, IntCallback onReconnect, VoidCallback onPing, VoidCallback onPong, StringCallback onMessage);

        [DllImport("__Internal")]
        private static extern void WebSocketConnect();

        [DllImport("__Internal")]
        private static extern void WebSocketDisconnect();

        [DllImport("__Internal")]
        private static extern void WebSocketSend(string message);

        [DllImport("__Internal")]
        private static extern bool WebSocketConnected();

        [DllImport("__Internal")]
        private static extern bool WebSocketDisconnected();
    }
}
#endif

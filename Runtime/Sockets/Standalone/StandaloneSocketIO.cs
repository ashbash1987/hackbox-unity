#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;

namespace Hackbox
{
    internal class StandaloneSocketIO : ISocketIO
    {
        internal StandaloneSocketIO(string uri, int engineVersion, Dictionary<string, string> queryParameters)
        {
            Socket = new SocketIO(uri, new SocketIOOptions() { EIO = engineVersion, Query = queryParameters });

            Socket.OnConnected += (sender, e) => OnConnected?.Invoke();
            Socket.OnError += (sender, e) => OnError?.Invoke(e);
            Socket.OnDisconnected += (sender, e) => OnDisconnected?.Invoke(e);
            Socket.OnReconnectAttempt += (sender, e) => OnReconnectAttempt?.Invoke(e);
            Socket.OnReconnected += (sender, e) => OnReconnected?.Invoke(e);
            Socket.OnReconnectFailed += (sender, e) => OnReconnectFailed?.Invoke();
            Socket.OnPing += (sender, e) => OnPing?.Invoke();
            Socket.OnPong += (sender, e) => OnPong?.Invoke(e);
        }

        private readonly SocketIO Socket;

        public event Action OnConnected;
        public event Action<string> OnError;
        public event Action<string> OnDisconnected;
        public event Action<int> OnReconnectAttempt;
        public event Action<int> OnReconnected;
        public event Action OnReconnectFailed;
        public event Action OnPing;
        public event Action<TimeSpan> OnPong;

        public bool Connected => Socket.Connected;
        public bool Disconnected => Socket.Disconnected;

        public async Task Connect()
        {
            await Socket.ConnectAsync();
        }

        public async Task Disconnect()
        {
            await Socket.DisconnectAsync();
        }

        public async Task Emit(string eventName, string message)
        {
            await Socket.EmitAsync(eventName, message);
        }

        public void On(string eventName, Action<JObject> messageHandler)
        {
            Socket.On(eventName, x =>
            {
                messageHandler?.Invoke(x.GetValue<JObject>());
            });
        }

        public void Off(string eventName)
        {
            Socket.Off(eventName);
        }
    }
}
#endif

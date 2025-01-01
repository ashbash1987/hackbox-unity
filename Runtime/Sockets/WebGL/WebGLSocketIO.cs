#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AOT;

namespace Hackbox
{
    internal class WebGLSocketIO : ISocketIO
    {
        private static readonly Dictionary<int, Action<JObject>> MessageHandlers = new Dictionary<int, Action<JObject>>();
        private static readonly List<WebGLSocketIO> WebSockets = new List<WebGLSocketIO>();

        internal WebGLSocketIO(string uri, int engineVersion, Dictionary<string, string> queryParameters)
        {
            WebSockets.Add(this);
            JObject queryObject = new JObject();
            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                queryObject[queryParameter.Key] = queryParameter.Value;
            }

            WebSocketInit(uri,
                          engineVersion,
                          queryObject.ToString(),
                          DelegateOnConnect,
                          DelegateOnError,
                          DelegateOnDisconnect,
                          DelegateOnReconnectAttempt,
                          DelegateOnReconnect,
                          DelegateOnReconnectFail,
                          DelegateOnPing,
                          DelegateOnPong);
        }

        ~WebGLSocketIO()
        {
            WebSockets.Remove(this);
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
            WebSocketEmit(eventName, message);
            return Task.CompletedTask;
        }

        public void On(string eventName, Action<JObject> messageHandler)
        {
            int handleIndex = MessageHandlers.Count;
            MessageHandlers[handleIndex] = messageHandler;

            WebSocketOn(eventName, handleIndex, DelegateOnMessage);
        }

        public void Off(string eventName)
        {
            WebSocketOff(eventName);
        }

        [MonoPInvokeCallback(typeof(VoidCallback))]
        public static void DelegateOnConnect()
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnConnected?.Invoke();
            }
        }

        [MonoPInvokeCallback(typeof(ErrorCallback))]
        public static void DelegateOnError(string error)
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnError?.Invoke(error);
            }
        }

        [MonoPInvokeCallback(typeof(ErrorCallback))]
        public static void DelegateOnDisconnect(string error)
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnDisconnected?.Invoke(error);
            }
        }

        [MonoPInvokeCallback(typeof(ReconnectCallback))]
        public static void DelegateOnReconnectAttempt(int attempt)
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnReconnectAttempt?.Invoke(attempt);
            }
        }

        [MonoPInvokeCallback(typeof(ReconnectCallback))]
        public static void DelegateOnReconnect(int attempt)
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnReconnected?.Invoke(attempt);
            }
        }

        [MonoPInvokeCallback(typeof(VoidCallback))]
        public static void DelegateOnReconnectFail()
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnReconnectFailed?.Invoke();
            }
        }

        [MonoPInvokeCallback(typeof(VoidCallback))]
        public static void DelegateOnPing()
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnPing?.Invoke();
            }
        }

        [MonoPInvokeCallback(typeof(VoidCallback))]
        public static void DelegateOnPong()
        {
            foreach (WebGLSocketIO webSocket in WebSockets)
            {
                webSocket.OnPong?.Invoke(TimeSpan.Zero);
            }
        }

        [MonoPInvokeCallback(typeof(MessageCallback))]
        public static void DelegateOnMessage(int handleIndex, string message)
        {
            if (MessageHandlers.TryGetValue(handleIndex, out Action<JObject> messageHandler))
            {
                messageHandler?.Invoke(JObject.Parse(message));
            }
        }

        private delegate void VoidCallback();
        private delegate void ErrorCallback(string error);
        private delegate void ReconnectCallback(int attempt);
        private delegate void MessageCallback(int handleIndex, string message);

        [DllImport("__Internal")]
        private static extern void WebSocketInit(string uri, int protocol, string query, VoidCallback onConnect, ErrorCallback onError, ErrorCallback onDisconnect, ReconnectCallback onReconnectAttempt, ReconnectCallback onReconnect, VoidCallback onReconnectFail, VoidCallback onPing, VoidCallback onPong);

        [DllImport("__Internal")]
        private static extern void WebSocketConnect();

        [DllImport("__Internal")]
        private static extern void WebSocketDisconnect();

        [DllImport("__Internal")]
        private static extern void WebSocketEmit(string eventName, string message);

        [DllImport("__Internal")]
        private static extern void WebSocketOn(string eventName, int handleIndex, MessageCallback callback);

        [DllImport("__Internal")]
        private static extern void WebSocketOff(string eventName);

        [DllImport("__Internal")]
        private static extern bool WebSocketConnected();

        [DllImport("__Internal")]
        private static extern bool WebSocketDisconnected();
    }
}
#endif

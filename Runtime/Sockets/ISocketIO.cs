using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Hackbox
{
    internal interface ISocketIO
    {
        event Action OnConnected;
        event Action<string> OnError;
        event Action<string> OnDisconnected;
        event Action<int> OnReconnectAttempt;
        event Action<int> OnReconnected;
        event Action OnReconnectFailed;
        event Action OnPing;
        event Action<TimeSpan> OnPong;

        bool Connected { get; }
        bool Disconnected { get; }

        Task Connect();
        Task Disconnect();

        Task Emit(string eventName, JObject message);

        void On(string eventName, Action<JObject> messageHandler);
        void Off(string eventName);
    }
}

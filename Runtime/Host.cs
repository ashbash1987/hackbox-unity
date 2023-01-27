using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Hackbox
{
    public class Host : MonoBehaviour
    {
        #region Events
        [Tooltip("Called when a room is created.")]
        public RoomCodeEvent OnRoomCreated = new RoomCodeEvent();
        [Tooltip("Called when this host connects to a room.")]
        public RoomCodeEvent OnRoomConnected = new RoomCodeEvent();
        [Tooltip("Called when this host disconnects from a room.")]
        public RoomCodeEvent OnRoomDisconnected = new RoomCodeEvent();
        [Tooltip("Called when this host attempts to reconnect to a room.")]
        public RoomCodeEvent OnRoomReconnecting = new RoomCodeEvent();
        [Tooltip("Called when this host fails an attempt to reconnect to a room.")]
        public RoomCodeEvent OnRoomReconnectFailed = new RoomCodeEvent();
        [Tooltip("Called when a member joins the room.")]
        public MemberEvent OnMemberJoined = new MemberEvent();
        [Tooltip("Called when a member is kicked from the room.")]
        public MemberEvent OnMemberKicked = new MemberEvent();
        [Tooltip("Called when a member sends a message in the room.")]
        public MessageEvent OnMessage = new MessageEvent();
        [Tooltip("Called when a ping/pong event occurs for this host.")]
        public UnityEvent OnPingPong = new UnityEvent();

        public readonly MessageEventCollection MessageEvents = new MessageEventCollection();
        #endregion

        #region Public Fields
        [Tooltip("URL of server to connect to. Unless you know what you are doing, leave this as is.")]
        public string URL = "https://app.hackbox.ca/";
        [Tooltip("A specific host name for this host instance.")]
        public string HostName = null;
        [Tooltip("The host version to connect to. Valid values are 1 or 2.")]
        public int HostVersion = 1;
        [Tooltip("If true, then it will reload the previous host setup.")]
        public bool ReloadHost = false;
        [Tooltip("If true, then additional debugging logging will be shown.")]
        public bool Debugging = false;
        #endregion

        #region Public Properties
        public bool Connected => _socket?.Connected ?? false;
        public bool Disconnected => _socket?.Disconnected ?? true;

        public string RoomCode
        {
            get;
            private set;
        }

        public string UserID
        {
            get;
            private set;
        }

        public Member[] AllMembers => Members.Values.ToArray();
        public bool HasMembers => Members.Any();
        #endregion

        #region Private Constants
        private const string AppName = "Hackbox.ca";
        private const string TemporaryFileName = "LastHackboxRoom-{Name}.json";
        #endregion

        #region Private Properties
        private string SocketURL => URL;
        private string RoomsURL => $"{URL}rooms/";
        private string TemporaryFilePath => Path.Combine(Application.temporaryCachePath, TemporaryFileName.Replace("{Name}", string.IsNullOrEmpty(HostName) ? name : HostName));
        #endregion

        #region Private Fields
        private readonly ConcurrentDictionary<string, Member> Members = new ConcurrentDictionary<string, Member>();
        private readonly ConcurrentQueue<Action> ThreadSafeActions = new ConcurrentQueue<Action>();
        private ISocketIO _socket = null;
        #endregion

        #region Unity Events
        protected virtual void Awake()
        {
            UserID = Guid.NewGuid().ToString();
        }

        protected virtual void Start()
        {
            Connect();
        }

        protected virtual void Update()
        {
            while (!ThreadSafeActions.IsEmpty)
            {
                if (ThreadSafeActions.TryDequeue(out Action action))
                {
                    action?.Invoke();
                }
            }
        }

        protected virtual void OnDestroy()
        {
            Disconnect();
        }
        #endregion

        #region Public Methods
        public void Connect(bool forceNewRoom = false)
        {
            IEnumerator Sequence()
            {
                if (forceNewRoom)
                {
                    RoomCode = null;
                }
                else if (ReloadHost)
                {
                    LoadRoomData();
                }

                if (string.IsNullOrEmpty(RoomCode))
                {
                    yield return GenerateRoom();
                }

                Save();
                _ = RestartSocket();
            }

            StartCoroutine(Sequence());
        }

        public void Disconnect()
        {
            _ = EndSocket();
        }

        public Member GetMemberByName(string name)
        {
            return Members.Values.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public Member GetMemberByID(string userID)
        {
            return Members.Values.FirstOrDefault(x => x.UserID.Equals(userID));
        }

        public void UpdateMemberState(Member member, State state)
        {
            member.State = state;
            SendMemberUpdate(new JValue(member.UserID), state.GenerateJSON(HostVersion));
        }

        public void UpdateMemberStates(IEnumerable<Member> members, State state)
        {
            foreach (Member member in members)
            {
                member.State = state;
            }

            SendMemberUpdate(new JArray(members.Select(x => x.UserID).ToArray()), state.GenerateJSON(HostVersion));
        }
 
        public void UpdateAllMemberStates(State state)
        {
            foreach (Member member in AllMembers)
            {
                member.State = state;
            }

            SendMemberUpdate(new JArray(AllMembers.Select(x => x.UserID).ToArray()), state.GenerateJSON(HostVersion));
        }
        #endregion

        #region Private Methods
        private void DoUnityAction(Action action)
        {
            ThreadSafeActions.Enqueue(action);
        }

        private void Log(string message)
        {
            DoUnityAction(() => Debug.Log(message));
        }

        private void LogWarn(string message)
        {
            DoUnityAction(() => Debug.LogWarning(message));
        }

        private void LogError(string message)
        {
            DoUnityAction(() => Debug.LogError(message));
        }

        private void LoadRoomData()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            try
            {
                if (!File.Exists(TemporaryFilePath))
                {
                    return;
                }

                string data = File.ReadAllText(TemporaryFilePath);
                JObject jData = JObject.Parse(data);
                RoomCode = (string)jData["roomCode"];
                UserID = (string)jData["userID"];
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
#endif
        }

        private void Save()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            JObject jData = JObject.FromObject(new
            {
                roomCode = RoomCode,
                userID = UserID
            });

            File.WriteAllText(TemporaryFilePath, jData.ToString());
#endif
        }

        private IEnumerator GenerateRoom()
        {
            JObject postData = new JObject(
                new JProperty("hostId", UserID)
            );

            Log($"Attempting room creation request to {AppName}...");
            using (UnityWebRequest request = UnityWebRequest.Put(RoomsURL, Encoding.UTF8.GetBytes(postData.ToString())))
            {
                request.method = "POST";
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                if (request.isHttpError || request.isNetworkError)
                {
                    Log($"Failed to request a room: {request.error}");
                    yield break;
                }

                JObject response = JObject.Parse(request.downloadHandler.text);
                if (response["ok"].Value<bool>())
                {
                    RoomCode = response["roomCode"].Value<string>();
                    Log($"Successfully created room <b>{RoomCode}</b> for host <i>{UserID}</i>.");
                    DoUnityAction(() => OnRoomCreated.Invoke(RoomCode));
                }                
            }
        }

        private async Task StartSocket()
        {
            Log($"Attempting socket connection to {AppName} for <b>{RoomCode}</b> with host <i>{UserID}</i>...");

            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
                ["userId"] = UserID,
                ["roomCode"] = RoomCode
            };

#if UNITY_EDITOR || UNITY_STANDALONE
            _socket = new StandaloneSocketIO(SocketURL, 4, queryParameters);
#elif UNITY_WEBGL
            _socket = new WebGLSocketIO(SOCKET_URL, 4, queryParameters);
#endif

            _socket.OnConnected += OnConnected;
            _socket.OnError += OnError;
            _socket.OnDisconnected += OnDisconnected;
            _socket.OnReconnectAttempt += OnReconnectAttempt;
            _socket.OnReconnected += OnReconnected;
            _socket.OnReconnectFailed += OnReconnectFailed;
            _socket.OnPing += OnPing;
            _socket.OnPong += OnPong;

            await _socket.Connect();
        }

        private void OnConnected()
        {
            Log($"Connected to {AppName} <b>{RoomCode}</b> with host <i>{UserID}</i>.");
            DoUnityAction(() =>
            {
                OnRoomConnected.Invoke(RoomCode);
            });

            _socket.On("state.host", OnHostStateUpdate);
            _socket.On("msg", OnMemberMessage);
        }

        private void OnError(string error)
        {
            LogError($"Failed connection to {AppName}: {error}");
        }

        private void OnDisconnected(string error)
        {
            LogWarn($"Disconnected from {AppName}: {error}");
            DoUnityAction(() =>
            {
                OnRoomDisconnected.Invoke(RoomCode);
            });
        }

        private void OnReconnectAttempt(int attemptCount)
        {
            Log($"Reconnecting to {AppName} <b>{RoomCode}</b> (attempt {attemptCount})...");
            DoUnityAction(() =>
            {
                OnRoomReconnecting.Invoke(RoomCode);
            });
        }

        private void OnReconnected(int attemptCount)
        {
            Log($"Reconnected to {AppName} <b>{RoomCode}</b> with host <i>{UserID}</i> (attempt {attemptCount}).");

            DoUnityAction(() =>
            {
                OnRoomConnected.Invoke(RoomCode);

                foreach (Member member in Members.Values)
                {
                    if (member.State != null)
                    {
                        UpdateMemberState(member, member.State);
                    }
                }
            });
        }

        private void OnReconnectFailed()
        {
            LogError($"Reconnect to {AppName} <b>{RoomCode}</b> failed.");
            DoUnityAction(() =>
            {
                OnRoomReconnectFailed.Invoke(RoomCode);
            });
        }

        private void OnPing()
        {
            if (Debugging)
            {
                Log($"Ping...");
            }
        }

        private void OnPong(TimeSpan e)
        {
            if (Debugging)
            {
                Log($"...Pong.");
            }

            DoUnityAction(() => OnPingPong.Invoke());           
        }

        private async Task EndSocket()
        {
            Log($"Closing socket connection to {AppName}...");

            if (_socket != null)
            {
                await _socket.Disconnect();
                _socket = null;
            }

            Log("Closed socket connection.");
        }

        private async Task RestartSocket()
        {
            if (_socket != null)
            {
                await EndSocket();
            }

            await StartSocket();
        }

        private void SendMemberUpdate(JToken to, JObject statePayload)
        {
            if (_socket == null)
            {
                LogError("Cannot send a member update - socket has not been initialised!");
                return;
            }

            JObject obj = new JObject(
                new JProperty("to", to),
                new JProperty("data", statePayload)
            );

            _ = _socket.Emit("member.update", obj);

            if (Debugging)
            {
                Log($"Emitting...\n{obj.ToString(Formatting.None)}");
            }
        }

        private void OnHostStateUpdate(JObject msgObject)
        {
            HashSet<Member> oldMembers = new HashSet<Member>(Members.Values);

            JObject membersObject = (JObject)msgObject["members"];
            foreach (JProperty memberProperty in membersObject.Properties())
            {
                JObject memberData = (JObject)memberProperty.Value;
                string userID = memberProperty.Name;
                if (Members.TryGetValue(userID, out Member member))
                {
                    member.Update(memberData);
                    oldMembers.Remove(member);
                }
                else
                {
                    Member newMember = new Member(memberData);
                    Members[newMember.UserID] = newMember;

                    Log($"Member <b>{newMember.Name}</b> <i>{newMember.UserID}</i> connected to room <b>{RoomCode}</b>.");
                    DoUnityAction(() => OnMemberJoined.Invoke(newMember));
                }
            }

            foreach (Member oldMember in oldMembers)
            {
                Log($"<b>{oldMember.Name}</b> has been kicked from room <b>{RoomCode}</b>.");
                if (Members.TryRemove(oldMember.UserID, out _))
                {
                    DoUnityAction(() => OnMemberKicked.Invoke(oldMember));
                }
            }
        }

        private void OnMemberMessage(JObject msgObject)
        {
            if (Debugging)
            {
                Log($"Receiving...\n{msgObject.ToString(Formatting.None)}");
            }

            string from = (string)msgObject["from"];
            if (!Members.TryGetValue(from, out Member fromMember))
            {
                Debug.LogError($"Received a message from member <i>{from}</i> but there is no known associated Member object!");
                return;
            }

            string id = (string)msgObject["id"];
            long timestamp = (long)msgObject["timestamp"];

            Message message = new Message(fromMember, id, DateTimeOffset.FromUnixTimeMilliseconds(timestamp), (JObject)msgObject["message"]);
            Log($"<i>{fromMember.Name}</i> {message}");
            DoUnityAction(() =>
            {
                OnMessage.Invoke(message);
                fromMember.OnMessage.Invoke(message);
                if (!string.IsNullOrEmpty(message.Event))
                {
                    MessageEvents.Invoke(message.Event, message);
                    fromMember.MessageEvents.Invoke(message.Event, message);
                }
            });
        }
        #endregion
    }
}

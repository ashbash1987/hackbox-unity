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

namespace Hackbox
{
    public class Host : MonoBehaviour
    {
        #region Enums
        public enum DebugLevel
        {
            Off,
            Minimal,
            Full
        }
        #endregion

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
        [Tooltip("Called when a member sends a value change in the room.")]
        public MessageEvent OnValueChange = new MessageEvent();
        [Tooltip("Called when a ping/pong event occurs for this host.")]
        public UnityEvent OnPingPong = new UnityEvent();

        public readonly MessageEventCollection MessageEvents = new MessageEventCollection();
        public readonly MessageEventCollection ValueChangeEvents = new MessageEventCollection();
        #endregion

        #region Public Fields
        [Tooltip("URL of server to connect to. Unless you know what you are doing, leave this as is.")]
        public string URL = "https://app.hackbox.ca/";
        [Tooltip("A specific host name for this host instance.")]
        public string HostName = null;
        [Tooltip("If true, then it will reload the previous host setup.")]
        public bool ReloadHost = false;
        [Tooltip("If true, then the created room will only allow members with Twitch credentials to join the room.")]
        public bool TwitchRequired = false;
        [Tooltip("If true, will connect on the Start lifecycle event of this component. Defaults to true.")]
        public bool ConnectOnStart = true;
        [Tooltip("If true, will re-connect on the Enable lifecycle event of this component. Defaults to true.")]
        public bool ReconnectOnEnable = true;
        [Tooltip("If true, will disconnect on the Disable lifecycle event of this component. Defaults to false.")]
        public bool DisconnectOnDisable = false;
        [Tooltip("The level of logging that will be shown.")]
        public DebugLevel Debugging = DebugLevel.Minimal;
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

        public string JoinRoomURL => $"{URL}?room={RoomCode}";

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
        private bool _socketManuallyClosing = false;
        #endregion

        #region Unity Events
        protected virtual void Awake()
        {
            UserID = Guid.NewGuid().ToString();
        }

        protected virtual void Start()
        {
            if (!Connected && ConnectOnStart)
            {
                Connect();
            }
        }

        protected virtual void OnEnable()
        {
            if (!Connected && ReconnectOnEnable)
            {
                Connect();
            }
        }

        protected virtual void OnDisable()
        {
            if (Connected && DisconnectOnDisable)
            {
                Disconnect();
            }
        }

        protected virtual void Update()
        {
            try
            {
                while (!ThreadSafeActions.IsEmpty)
                {
                    if (ThreadSafeActions.TryDequeue(out Action action))
                    {
                        action?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        protected virtual void OnDestroy()
        {
            if (Connected)
            {
                Disconnect();
            }
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
                    if (!string.IsNullOrEmpty(RoomCode))
                    {
                        yield return CheckRoomExists();
                    }
                }

                if (string.IsNullOrEmpty(RoomCode))
                {
                    yield return GenerateRoom();
                    if (string.IsNullOrEmpty(RoomCode))
                    {
                        yield break;
                    }
                }

                Save();
                _ = RestartSocket();
            }

            StartCoroutine(Sequence());
        }

        public void ConnectExisting(string roomCode, string userID)
        {
            if (string.IsNullOrEmpty(roomCode) || string.IsNullOrEmpty(userID))
            {
                Connect();
                return;
            }

            IEnumerator Sequence()
            {
                RoomCode = roomCode;
                UserID = userID;

                yield return CheckRoomExists();

                if (string.IsNullOrEmpty(RoomCode))
                {
                    yield break;
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
            try
            {
                member.State = state;
                SendMemberUpdate(state, member);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public void UpdateMemberStates(IEnumerable<Member> members, State state)
        {
            try
            {
                foreach (Member member in members)
                {
                    member.State = state;
                }

                SendMemberUpdate(state, members);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
 
        public void UpdateAllMemberStates(State state)
        {
            try
            {
                foreach (Member member in AllMembers)
                {
                    member.State = state;
                }

                SendMemberUpdate(state, AllMembers);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        #endregion

        #region Private Methods
        private void DoUnityAction(Action action)
        {
            ThreadSafeActions.Enqueue(action);
        }

        private void VerboseLog(string message)
        {
            if (Debugging == DebugLevel.Full)
            {
                DoUnityAction(() => Debug.Log(message));
            }
        }

        private void Log(string message)
        {
            if (Debugging >= DebugLevel.Minimal)
            {
                DoUnityAction(() => Debug.Log(message));
            }
        }

        private void LogWarn(string message)
        {
            if (Debugging >= DebugLevel.Minimal)
            {
                DoUnityAction(() => Debug.LogWarning(message));
            }
        }

        private void LogError(string message)
        {
            DoUnityAction(() => Debug.LogError(message));
        }

        private void LogException(Exception exception)
        {
            DoUnityAction(() => Debug.LogException(exception));
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
                LogException(ex);
            }
#endif
        }

        private void Save()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            try
            {
                JObject jData = JObject.FromObject(new
                {
                    roomCode = RoomCode,
                    userID = UserID
                });

                File.WriteAllText(TemporaryFilePath, jData.ToString());
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
#endif
        }

        private IEnumerator CheckRoomExists()
        {
            Log($"Checking room <b>{RoomCode}</b> exists at {AppName}...");
            using (UnityWebRequest request = UnityWebRequest.Get($"{RoomsURL}{RoomCode}"))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        break;

                    default:
                        LogError($"Failed to check a room: {request.error}");
                        yield break;
                }
#else
                if (request.isHttpError || request.isNetworkError)
                {
                    LogError($"Failed to check a room: {request.error}");
                    yield break;
                }
#endif

                JObject response = JObject.Parse(request.downloadHandler.text);
                bool exists = response["exists"].Value<bool>();
                if (exists)
                {
                    Log($"Room <b>{RoomCode}</b> exists.");
                }
                else
                {
                    LogError($"Room <b>{RoomCode}</b> does not exist. Resetting stored room code and host user ID.");
                    RoomCode = null;
                    UserID = Guid.NewGuid().ToString();
                }
            }
        }

        private IEnumerator GenerateRoom()
        {
            JObject postData = new JObject(
                new JProperty("hostId", UserID),
                new JProperty("twitchRequired", TwitchRequired)
            );

            Log($"Attempting room creation request to {AppName}...");
            using (UnityWebRequest request = UnityWebRequest.Put(RoomsURL, Encoding.UTF8.GetBytes(postData.ToString())))
            {
                request.method = "POST";
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        break;

                    default:
                        LogError($"Failed to request a room: {request.error}");
                        yield break;
                }
#else
                if (request.isHttpError || request.isNetworkError)
                {
                    Log($"Failed to request a room: {request.error}");
                    yield break;
                }
#endif

                JObject response = JObject.Parse(request.downloadHandler.text);
                if (response["ok"].Value<bool>())
                {
                    RoomCode = response["roomCode"].Value<string>();
                    Log($"Successfully created room <b>{RoomCode}</b> for host <i>{UserID}</i>.");
                    DoUnityAction(() => OnRoomCreated.Invoke(RoomCode));
                }
                else
                {
                    LogError($"Failed to create room <b>{RoomCode}</b> for host <i>{UserID}</i>.");
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

            _socketManuallyClosing = false;            

#if UNITY_EDITOR || UNITY_STANDALONE
            _socket = new StandaloneSocketIO(SocketURL, 4, queryParameters);
#elif UNITY_WEBGL
            _socket = new WebGLSocketIO(SocketURL, 4, queryParameters);
#endif
            ListenForEvents();

            await _socket.Connect();
        }

        private void ListenForEvents()
        {
            _socket.OnConnected += OnConnected;
            _socket.OnError += OnError;
            _socket.OnDisconnected += OnDisconnected;
            _socket.OnReconnectAttempt += OnReconnectAttempt;
            _socket.OnReconnected += OnReconnected;
            _socket.OnReconnectFailed += OnReconnectFailed;
            _socket.OnPing += OnPing;
            _socket.OnPong += OnPong;
        }

        private void UnlistenForEvents()
        {
            _socket.OnConnected -= OnConnected;
            _socket.OnError -= OnError;
            _socket.OnDisconnected -= OnDisconnected;
            _socket.OnReconnectAttempt -= OnReconnectAttempt;
            _socket.OnReconnected -= OnReconnected;
            _socket.OnReconnectFailed -= OnReconnectFailed;
            _socket.OnPing -= OnPing;
            _socket.OnPong -= OnPong;
        }

        private void OnConnected()
        {
            try
            {
                if (_socket == null)
                {
                    LogError("Get OnConnected event, but socket has not been initialised!");
                    return;
                }

                Log($"Connected to {AppName} <b>{RoomCode}</b> with host <i>{UserID}</i>.");
                DoUnityAction(() =>
                {
                    OnRoomConnected.Invoke(RoomCode);
                });

                _socket.On("state.host", OnHostStateUpdate);
                _socket.On("msg", OnMemberMessage);
                _socket.On("change", OnMemberValueChange);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
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

                if (!_socketManuallyClosing)
                {
                    Connect();
                }
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
            VerboseLog($"Ping...");
        }

        private void OnPong(TimeSpan e)
        {
            VerboseLog($"...Pong.");

            DoUnityAction(() => OnPingPong.Invoke());           
        }

        private async Task EndSocket()
        {
            Log($"Closing socket connection to {AppName}...");
            _socketManuallyClosing = true;

            if (_socket != null)
            {
                await _socket.Disconnect();
                UnlistenForEvents();
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

        private void SendMemberUpdate(State state, Member member)
        {
            if (_socket == null)
            {
                LogError("Cannot send a member update - socket has not been initialised!");
                return;
            }

            StringWriter stringWriter = new StringWriter();
            JsonTextWriter json = new JsonTextWriter(stringWriter);

            json.WriteStartObject();
            {
                json.WritePropertyName("to");
                json.WriteValue(member.UserID);

                json.WritePropertyName("data");
                state.WriteJSON(json);
            }
            json.WriteEndObject();

            string data = stringWriter.ToString();
            _ = _socket.Emit("member.update", data);

            VerboseLog($"Emitting...\n{data}");
        }

        private void SendMemberUpdate(State state, IEnumerable<Member> members)
        {
            switch (members.Count())
            {
                case 0:
                    return;

                case 1:
                    SendMemberUpdate(state, members.First());
                    return;

                default:
                    if (_socket == null)
                    {
                        LogError("Cannot send a member update - socket has not been initialised!");
                        return;
                    }
                    break;
            }

            StringWriter stringWriter = new StringWriter();
            JsonTextWriter json = new JsonTextWriter(stringWriter);

            json.WriteStartObject();
            {
                json.WritePropertyName("to");
                json.WriteStartArray();
                foreach (Member member in members)
                {
                    json.WriteValue(member.UserID);
                }
                json.WriteEndArray();

                json.WritePropertyName("data");
                state.WriteJSON(json);
            }
            json.WriteEndObject();

            string data = stringWriter.ToString();
            _ = _socket.Emit("member.update", data);

            VerboseLog($"Emitting...\n{data}");
        }

        private void OnHostStateUpdate(JObject msgObject)
        {
            try
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
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void OnMemberMessage(JObject msgObject)
        {
            try
            {
                VerboseLog($"Receiving...\n{msgObject.ToString(Formatting.None)}");

                string from = (string)msgObject["from"];
                if (!Members.TryGetValue(from, out Member fromMember))
                {
                    LogError($"Received a message from member <i>{from}</i> but there is no known associated Member object!");
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
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void OnMemberValueChange(JObject msgObject)
        {
            try
            {
                VerboseLog($"Receiving...\n{msgObject.ToString(Formatting.None)}");

                string from = (string)msgObject["from"];
                if (!Members.TryGetValue(from, out Member fromMember))
                {
                    LogError($"Received a value change from member <i>{from}</i> but there is no known associated Member object!");
                    return;
                }

                string id = (string)msgObject["id"];
                long timestamp = (long)msgObject["timestamp"];

                Message message = new Message(fromMember, id, DateTimeOffset.FromUnixTimeMilliseconds(timestamp), (JObject)msgObject["message"]);
                Log($"<i>{fromMember.Name}</i> {message}");
                DoUnityAction(() =>
                {
                    OnValueChange.Invoke(message);
                    fromMember.OnValueChange.Invoke(message);
                    if (!string.IsNullOrEmpty(message.Event))
                    {
                        ValueChangeEvents.Invoke(message.Event, message);
                        fromMember.ValueChangeEvents.Invoke(message.Event, message);
                    }
                });
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }
        #endregion
    }
}

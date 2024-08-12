using System;
using Newtonsoft.Json.Linq;

namespace Hackbox
{
    public class Member
    {
        #region Identity Types
        public class TwitchIdentity : IEquatable<TwitchIdentity>
        {
            internal TwitchIdentity(JObject data)
            {
                Update(data);
            }

            public string ID
            {
                get;
                private set;
            }

            public string UserName
            {
                get;
                private set;
            }

            public string AvatarURL
            {
                get;
                private set;
            }

            public void Update(JObject data)
            {
                ID = (string)data["id"];
                UserName = (string)data["username"];
                AvatarURL = (string)data["photo"];
            }

            public override string ToString()
            {
                if (string.IsNullOrEmpty(ID))
                {
                    return "<Unknown>";
                }

                return $"{UserName} [{ID}]";
            }

            public override bool Equals(object obj)
            {
                if (obj is TwitchIdentity other)
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        return ID.Equals(other.ID);
                    }

                    return string.IsNullOrEmpty(other.ID);
                }

                return false;
            }

            public bool Equals(TwitchIdentity other)
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    return ID.Equals(other.ID);
                }

                return string.IsNullOrEmpty(other.ID);
            }

            public override int GetHashCode()
            {
                return ID?.GetHashCode() ?? UserName?.GetHashCode() ?? 0;
            }

            public static bool operator == (TwitchIdentity lhs, TwitchIdentity rhs)
            {
                if (Object.ReferenceEquals(lhs, null) && Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }

                if ((Object.ReferenceEquals(lhs, null) && !Object.ReferenceEquals(rhs, null)) ||
                    (Object.ReferenceEquals(rhs, null) && !Object.ReferenceEquals(lhs, null)))
                {
                    return false;
                }

                return lhs.Equals(rhs);
            }

            public static bool operator != (TwitchIdentity lhs, TwitchIdentity rhs)
            {
                return !(lhs == rhs);
            }
        }
        #endregion

        internal Member(JObject data)
        {
            Update(data);
            State = new State();
        }

        public MessageEvent OnMessage = new MessageEvent();
        public MessageEvent OnValueChange = new MessageEvent();

        public readonly MessageEventCollection MessageEvents = new MessageEventCollection();
        public readonly MessageEventCollection ValueChangeEvents = new MessageEventCollection();

        public string UserID
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public TwitchIdentity Twitch
        {
            get;
            private set;
        }

        public State State
        {
            get;
            internal set;
        }

        public void Update(JObject data)
        {
            UserID = (string)data["id"];
            Name = (string)data["name"];

            if (data.ContainsKey("metadata"))
            {
                UpdateMetadata((JObject)data["metadata"]);
            }            
        }

        public override int GetHashCode()
        {
            return UserID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Member otherMember)
            {
                return UserID == otherMember.UserID;
            }

            return false;
        }

        private void UpdateMetadata(JObject metaData)
        {
            if (metaData.ContainsKey("twitch"))
            {
                JObject twitchData = (JObject)metaData["twitch"];
                if (Twitch == null)
                {
                    Twitch = new TwitchIdentity(twitchData);
                }
                else
                {
                    Twitch.Update(twitchData);
                }
            }
        }
    }
}

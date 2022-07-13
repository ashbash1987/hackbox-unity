using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hackbox
{
    public class Member
    {
        internal Member(JObject data)
        {
            Update(data);
            State = new State();
        }

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

        public State State
        {
            get;
            internal set;
        }

        public void Update(JObject data)
        {
            UserID = (string)data["id"];
            Name = (string)data["name"];
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
    }
}

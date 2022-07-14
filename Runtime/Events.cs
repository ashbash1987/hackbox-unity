using System;
using UnityEngine.Events;

namespace Hackbox
{
    [Serializable]
    public class RoomCodeEvent : UnityEvent<string>
    {
        public RoomCodeEvent()
        {
        }
    }

    [Serializable]
    public class MemberEvent : UnityEvent<Member>
    {
        public MemberEvent()
        {
        }
    }

    [Serializable]
    public class MessageEvent : UnityEvent<Message>
    {
        public MessageEvent()
        {
        }
    }
}

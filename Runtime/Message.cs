using System;
using Newtonsoft.Json.Linq;

namespace Hackbox
{
    public sealed class Message
    {
        internal Message(Member member, string id, DateTimeOffset time, JObject messageData)
        {
            Member = member;
            ID = id;
            Time = time;
            MessageData = messageData;
        }

        public readonly Member Member;
        public readonly string ID;
        public readonly DateTimeOffset Time;
        public readonly JObject MessageData;

        public string Event
        {
            get => MessageData.ContainsKey("event") ? (string)MessageData["event"] : null;
        }

        public string Value
        {
            get => MessageData.ContainsKey("value") ? (string)MessageData["value"] : null;
        }

        public override string ToString()
        {
            return $"[{Time}|{ID}]: {Event}, {Value}";
        }
    }
}
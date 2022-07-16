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
            Timestamp = time;
            MessageData = messageData;
        }

        public readonly Member Member;
        public readonly string ID;
        public readonly DateTimeOffset Timestamp;
        public readonly JObject MessageData;

        public string Event
        {
            get => MessageData.ContainsKey("event") ? (string)MessageData["event"] : null;
        }

        public int Milliseconds
        {
            get => MessageData.ContainsKey("ms") ? (int)MessageData["ms"] : 0;
        }

        public string Value
        {
            get => MessageData.ContainsKey("value") ? (string)MessageData["value"] : null;
        }

        public override string ToString()
        {
            return $"[{Timestamp}|{ID}]: {Event}, {Value}";
        }
    }
}
using System;
using System.Linq;
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
            get
            {
                string[] values = Values;
                if (values == null || values.Length == 0)
                {
                    return null;
                }
                if (values.Length == 1)
                {
                    return values[0];
                }

                return string.Join(",", values);
            }
        }

        public string[] Values
        {
            get
            {
                if (!MessageData.TryGetValue("value", out JToken value))
                {
                    return null;
                }

                if (value is JArray valueArray)
                {
                    return valueArray.Values<string>().ToArray();
                }

                if (value is JValue)
                {
                    return new[] { (string)value };
                }

                if (value is JObject)
                {
                    return new[] { value.ToString() };
                }

                return null;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Value))
            {
                return $"[{Timestamp}|{ID}]: <b>{Event}</b>";
            }

            return $"[{Timestamp}|{ID}]: <b>{Event}</b>=[<b>{Value}</b>]";
        }
    }
}

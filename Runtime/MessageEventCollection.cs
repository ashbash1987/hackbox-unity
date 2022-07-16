using System.Collections.Generic;
using UnityEngine.Events;

namespace Hackbox
{
    public sealed class MessageEventCollection
    {
        public MessageEvent this[string eventName]
        {
            get => GetMessageEvent(eventName);
        }

        private readonly Dictionary<string, MessageEvent> Events = new Dictionary<string, MessageEvent>();

        public void AddListener(string eventName, UnityAction<Message> listener)
        {
            GetMessageEvent(eventName).AddListener(listener);
        }

        public void RemoveListener(string eventName, UnityAction<Message> listener)
        {
            GetMessageEvent(eventName).RemoveListener(listener);
        }

        public void RemoveAllListeners(string eventName)
        {
            GetMessageEvent(eventName).RemoveAllListeners();
        }

        public void Invoke(string eventName, Message message)
        {
            GetMessageEvent(eventName, false)?.Invoke(message);
        }

        private MessageEvent GetMessageEvent(string eventName, bool create = true)
        {
            if (!Events.TryGetValue(eventName, out MessageEvent messageEvent))
            {
                messageEvent = new MessageEvent();
                Events[eventName] = messageEvent;
            }

            return messageEvent;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Hackbox
{
    public class MessageEventQueue : MonoBehaviour
    {
        public Host Host = null;
        public Member[] Members = null;
        public string[] EventNames = null;

        public bool ClearOnEnable = false;
        public bool ClearOnDisable = false;

        public bool HasMessages => MessageQueue.Count > 0;

        private readonly Queue<Message> MessageQueue = new Queue<Message>();
        private Member[] _currentMembers = null;

        private void OnEnable()
        {
            if (ClearOnEnable)
            {
                Clear();
            }
            CreateListeners();
        }

        private void OnDisable()
        {
            if (ClearOnDisable)
            {
                Clear();
            }
            DestroyListeners();
        }

        private void OnDestroy()
        {
            DestroyListeners();
        }

        private void Update()
        {
            if (Members != _currentMembers)
            {
                DestroyListeners();
                _currentMembers = Members;
                CreateListeners();
            }
        }

        /// <summary>
        /// Returns the next message in the queue, and removes it from the queue collection.
        /// </summary>
        /// <returns>The next message in the queue.</returns>
        public Message Dequeue()
        {
            return MessageQueue.Dequeue();
        }

        /// <summary>
        /// Returns the next message in the queue, but does not remove it from the queue collection.
        /// </summary>
        /// <returns>The next message in the queue.</returns>
        public Message Peek()
        {
            return MessageQueue.Peek();
        }

        /// <summary>
        /// Clears the message queue.
        /// </summary>
        public void Clear()
        {
            MessageQueue.Clear();
        }

        private void CreateListeners()
        {
            foreach (string eventName in EventNames)
            {
                if (_currentMembers != null)
                {
                    foreach (Member member in _currentMembers)
                    {
                        member.MessageEvents.AddListener(eventName, OnMessage);
                    }
                }
                else
                {
                    Host.MessageEvents.AddListener(eventName, OnMessage);
                }
            }
        }

        private void DestroyListeners()
        {
            foreach (string eventName in EventNames)
            {
                if (_currentMembers != null)
                {
                    foreach (Member member in _currentMembers)
                    {
                        member.MessageEvents.RemoveListener(eventName, OnMessage);
                    }
                }
                else
                {
                    Host.MessageEvents.RemoveListener(eventName, OnMessage);
                }                
            }
        }

        private void OnMessage(Message message)
        {
            MessageQueue.Enqueue(message);
        }
    }
}

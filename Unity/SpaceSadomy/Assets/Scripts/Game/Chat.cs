using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using ServerClientCommon;

namespace Nebula
{
    public class Chat
    {
        private Dictionary<string, ChatMessage> messages;
        private int _maxMessages;

        private int countOfNewMessages = 0;

        public Chat(int maxMessages) 
        {
            _maxMessages = maxMessages;
            this.messages = new Dictionary<string,ChatMessage>();
        }

        private MessageTimeComparer comparer = new MessageTimeComparer();

        public class MessageTimeComparer : IComparer<ChatMessage> {
            public int Compare(ChatMessage x, ChatMessage y) {
                return x.Time.CompareTo(y.Time);
            }
        }

        public List<ChatMessage> Messages() 
        {
            List<ChatMessage> messageList = new List<ChatMessage>();
            foreach (var kv in this.messages) {
                messageList.Add(kv.Value);
            }
            messageList.Sort(comparer);
            return messageList;
        }

        private ChatMessage FirstMessage()
        {
            List<ChatMessage> messageList = new List<ChatMessage>();
            foreach (var kv in this.messages) {
                messageList.Add(kv.Value);
            }
            messageList.Sort(comparer);
            if (messageList.Count == 0) {
                return null;
            }
            return messageList[0];
        }

        public int CountOfNewMessages()
        {
            return this.countOfNewMessages;
        }

        public void ResetCountOfNewMessages()
        {
            this.countOfNewMessages = 0; 
        }

        private void AddMessage(ChatMessage message) 
        {
            if(!this.messages.ContainsKey(message.Id))
            {
                if (messages.Count >= _maxMessages) 
                {
                    this.messages.Remove(FirstMessage().Id);
                }
                this.messages.Add(message.Id, message);
                countOfNewMessages++;
                Events.OnNewChatMessageAdded(message);
            }
        }

        //return most recent message with biggest Time property
        private float GetMostRecentMessageTime() {
            float time = 0f;
            foreach (var pMessage in this.messages) {
                if (pMessage.Value.Time > time) {
                    time = pMessage.Value.Time;
                }
            }
            return time;
        }

        private float GenerateTimeForLocalMessage() {
            return this.GetMostRecentMessageTime() + Time.deltaTime;
        }

        public void PastLocalMessage(string message) {
            string id = System.Guid.NewGuid().ToString();
            float time = this.GenerateTimeForLocalMessage();

            Hashtable messageHash = new Hashtable {
                {(int)SPC.ChatMessageId, id },
                {(int)SPC.ChatMessageGroup, ChatGroup.local.toByte() },
                {(int)SPC.ChatMessage, message },
                {(int)SPC.ChatSourceLogin, string.Empty },
                {(int)SPC.ChatSourceName, string.Empty },
                {(int)SPC.ChatReceiverLogin, string.Empty },
                {(int)SPC.ChatMessageTime, time }
            };
            this.AddMessage(new ChatMessage(messageHash));
        }


        public void ReceiveUpdate(Hashtable messages) 
        {

            foreach (DictionaryEntry entry in messages)
            {
                Hashtable msgInfo = entry.Value as Hashtable;
                if (msgInfo != null)
                {
                    var msg = new ChatMessage(msgInfo);
                    this.AddMessage(msg);
                    Events.OnChatMessageReceived(msg);
                }
            }
        }
    }
}

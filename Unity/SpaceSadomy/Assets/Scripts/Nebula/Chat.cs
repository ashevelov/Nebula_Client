using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using ServerClientCommon;

namespace Nebula
{
    public class Chat
    {
        private List<ChatMessage> mMessages = new List<ChatMessage>();
        public const int MAX_MESSAGES = 100;

        private readonly Dictionary<ChatGroup, Color> messageColors;

        public Chat() {
            messageColors = new Dictionary<ChatGroup, Color> {
                { ChatGroup.group, Color.cyan       },
                { ChatGroup.guild, Color.green      },
                { ChatGroup.local, Color.yellow     },
                { ChatGroup.whisper, Color.blue     },
                { ChatGroup.zone, Color.white       }
            };
        }


        public void AddMessage(ChatMessage message) {
            mMessages.Add(message);
            if(mMessages.Count > 100) {
                mMessages.RemoveAt(0);
            }
            Debug.LogFormat("message added: {0}", mMessages.Count);
            Events.OnChatMessageReceived(message);
        }

        public List<ChatMessage> messages {
            get {
                return mMessages;
            }
        }

        public void PastLocalMessage(string text) {
            AddMessage(new ChatMessage {
                chatGroup = ChatGroup.local, links = new List<ChatLinkedObject>(), message = text,
                messageID = System.Guid.NewGuid().ToString(), sourceCharacterID = string.Empty, sourceLogin = string.Empty, targetCharacterID = string.Empty, targetLogin = string.Empty
            });
        }

        public Color MessageColor(ChatGroup group) {
            return messageColors[group];
        }
    }
}

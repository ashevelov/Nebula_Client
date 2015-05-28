namespace Nebula
{
    using Common;
    using UnityEngine;
    using System.Collections;
    using Nebula;

    /// <summary>
    /// Hold server message
    /// </summary>
    public class ChatMessage : IInfo  {

        public string Id { get; private set; }
        public ChatGroup Group { get; private set; }
        public string Message { get; private set; }
        public string Sender { get; private set; }
        public string SenderName { get; private set; }

        public string Receiver { get; private set; }

        public float Time { get; private set; }

         public ChatMessage(Hashtable info )
        {
            this.ParseInfo(info);
        }

        public System.Collections.Hashtable GetInfo()
        {
            Hashtable info = new Hashtable();
            info.Add(GenericEventProps.ChatMessageId, this.Id);
            info.Add(GenericEventProps.ChatMessageGroup, this.Group.toByte());
            info.Add(GenericEventProps.ChatMessage, this.Message);
            info.Add(GenericEventProps.ChatSourceLogin, this.Sender);
            info.Add(GenericEventProps.ChatMessageSourceName, this.SenderName);
            info.Add(GenericEventProps.ChatReceiverLogin, this.Receiver);
            info.Add(GenericEventProps.ChatMessageTime, this.Time);
            return info;
        }

        public void ParseInfo(System.Collections.Hashtable info)
        {
            this.Id = info.GetValue<string>(GenericEventProps.ChatMessageId, string.Empty);
            this.Group = (ChatGroup)info.GetValue<byte>(GenericEventProps.ChatMessageGroup, (byte)0);
            this.Message = info.GetValue<string>(GenericEventProps.ChatMessage, string.Empty);
            this.Sender = info.GetValue<string>(GenericEventProps.ChatSourceLogin, string.Empty);
            this.SenderName = info.GetValue<string>(GenericEventProps.ChatMessageSourceName, string.Empty);
            this.Receiver = info.GetValue<string>(GenericEventProps.ChatReceiverLogin, string.Empty);
            this.Time = info.GetValue<float>(GenericEventProps.ChatMessageTime, 0.0f);
        }

        public string DecoratedMessage 
        {
            get {
                string decoratedMessage = string.Empty;
                switch (this.Group) {
                    case ChatGroup.all:
                        {
                            decoratedMessage = string.Format("[{0}][@{1}:]{2}", string.Empty, this.SenderName.Trim(), this.Message.Color(Color.gray));
                        }
                        break;
                    case ChatGroup.alliance:
                        {
                            decoratedMessage = string.Format("[{0}][@{1}:]{2}", string.Empty, this.SenderName, this.Message.Color(Color.green));
                        }
                        break;
                    case ChatGroup.zone:
                        {
                            decoratedMessage = string.Format("[{0}][@{1}:]{2}", string.Empty, this.SenderName, this.Message.Color(Color.green));
                        }
                        break;
                    case ChatGroup.group:
                        {
                            decoratedMessage = string.Format("[{0}][@{1}:]{2}", string.Empty, this.SenderName, this.Message.Color("yellow"));
                        }
                        break;
                    case ChatGroup.me:
                        {
                            string meName = (MmoEngine.Get.Game.Avatar != null) ? MmoEngine.Get.Game.Avatar.Name : "?";
                            decoratedMessage = string.Format("[{0}][@{1}][@{2}:]{3}", string.Empty, meName, this.SenderName, this.Message.Color(Color.clear));
                        }
                        break;
                    case ChatGroup.whisper:
                        {
                            string meName = (MmoEngine.Get.Game.Avatar != null) ? MmoEngine.Get.Game.Avatar.Name : "?";
                            decoratedMessage = string.Format("[{0}][@{1}][@{2}:]{3}", string.Empty, meName, this.SenderName, this.Message.Color(Color.blue));
                        }
                        break;
                    default:
                        {
                            decoratedMessage = string.Format("[{0}][@{1}:]{2}", string.Empty, this.SenderName, this.Message.Color(Color.white));
                        }
                        break;
                }
                return decoratedMessage;
            }
        }
    }
}
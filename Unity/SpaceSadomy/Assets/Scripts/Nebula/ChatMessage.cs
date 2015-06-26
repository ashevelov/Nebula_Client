namespace Nebula
{
    using Common;
    using UnityEngine;
    using System.Collections;
    using Nebula;
    using ServerClientCommon;
    using System.Collections.Generic;

    /// <summary>
    /// Hold server message
    /// </summary>
    public class ChatMessage  {

        public string messageID { get; set; }
        public string sourceLogin { get; set; }
        public string sourceCharacterID { get; set; }
        public string targetLogin { get; set; }
        public string targetCharacterID { get; set; }
        public ChatGroup chatGroup { get; set; }
        public string message { get; set; }
        public List<ChatLinkedObject> links { get; set; }

        public string DecoratedMessage {
            get {
                Color color = MmoEngine.Get.GameData.Chat.MessageColor(chatGroup);
                return string.Format("[{0}]: {1}", sourceLogin.Color(color), message.Color(color));
            }
        }
    }
}
namespace Nebula.Mmo.Games.Strategies.Events.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using System.Collections.Generic;

    public class ChatMessageEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            string messageID = (string)eventData.Parameters[(byte)ParameterCode.MessageId];
            string sourceLogin = (string)eventData.Parameters[(byte)ParameterCode.SourceLogin];
            string sourceCharacterID = (string)eventData.Parameters[(byte)ParameterCode.SourceCharacterId];
            string targetLogin = (string)eventData.Parameters[(byte)ParameterCode.TargetLogin];
            string targetCharacterID = (string)eventData.Parameters[(byte)ParameterCode.CharacterId];
            ChatGroup chatGroup = (ChatGroup)(int)eventData.Parameters[(byte)ParameterCode.Group];
            string message = (string)eventData.Parameters[(byte)ParameterCode.Body];
            object[] links = (object[])eventData.Parameters[(byte)ParameterCode.Attachments];

            List<ChatLinkedObject> linkedObjects = new List<ChatLinkedObject>();
            if(links != null ) {
                foreach(object link in links) {
                    ChatLinkedObject linkedObject = new ChatLinkedObject();
                    linkedObject.ParseInfo(link as Hashtable);
                    linkedObjects.Add(linkedObject);
                }
            }

            ChatMessage messageObject = new ChatMessage {
                chatGroup = chatGroup,
                links = linkedObjects,
                message = message,
                messageID = messageID,
                sourceCharacterID = sourceCharacterID,
                sourceLogin = sourceLogin,
                targetCharacterID = targetCharacterID,
                targetLogin = targetLogin
            };
            game.Engine.GameData.Chat.AddMessage(messageObject);
        }

    }
}
namespace Nebula.Mmo.Games.Strategies.Events.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class UpdateNotificationsEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {

            Hashtable notificationHash = eventData.Parameters[(byte)ParameterCode.Notifications] as Hashtable;
            if(notificationHash == null ) {
                Debug.LogError("notifications is null");
                return;
            }

            game.Engine.GameData.notifications.ParseInfo(notificationHash);
        }
    }
}

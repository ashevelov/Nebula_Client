namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class HandleNotificationOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                return;
            }

            Hashtable notificationHash = response.Parameters[(byte)ParameterCode.Notifications] as Hashtable;
            if(notificationHash == null ) {
                Debug.LogError("notification hash is null");
                return;
            }

            game.Engine.GameData.notifications.ParseInfo(notificationHash);
        }
    }
}

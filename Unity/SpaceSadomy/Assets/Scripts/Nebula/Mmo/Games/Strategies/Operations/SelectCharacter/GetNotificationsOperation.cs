namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class GetNotificationsOperation : BaseOperationHandler {

        public override void Handle(BaseGame game, OperationResponse response) {
            if(false == CheckSuccess(game, response)) {
                return;
            }
            Hashtable notificationHash = response.Parameters[(byte)ParameterCode.Notifications] as Hashtable;
            if(notificationHash == null ) {
                Debug.LogError("notfications hash is null");
                return;
            }
            game.Engine.GameData.notifications.ParseInfo(notificationHash);
        }
    }
}

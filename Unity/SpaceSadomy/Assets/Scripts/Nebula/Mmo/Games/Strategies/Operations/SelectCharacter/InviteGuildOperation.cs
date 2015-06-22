namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;

    public class InviteGuildOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log(response.DebugMessage);
                return;
            }
        }
    }
}

namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {

    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class DeleteGuildOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("DeleteGuildOperation: error occured");
                return;
            }
            bool success = (bool)response.Parameters[(byte)ParameterCode.Result];
            Debug.Log("guild deleted with status: " + success);
        }
    }
}
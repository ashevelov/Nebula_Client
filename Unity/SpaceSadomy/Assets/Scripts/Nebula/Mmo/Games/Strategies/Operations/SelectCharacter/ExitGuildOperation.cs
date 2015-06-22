namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class ExitGuildOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("some error of exiting guild occured");
                return;
            }

            bool success = (bool)response.Parameters[(byte)ParameterCode.Result];
            Debug.LogFormat("guild exiting operation with status = {0}", success);
        }
    }
}

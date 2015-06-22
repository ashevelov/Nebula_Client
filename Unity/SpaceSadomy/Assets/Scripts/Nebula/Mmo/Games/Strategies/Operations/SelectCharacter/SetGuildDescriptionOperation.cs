namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;

    public class SetGuildDescriptionOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("Set guild description error");
                return;
            }
            Debug.Log("set guild description response...");
        }
    }
}

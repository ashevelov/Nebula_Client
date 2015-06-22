namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;

    public class ChangeGuildMemberStatusOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("some error of changinf member status");
                return;
            }
            Debug.Log("member status changed");
        }
    }
}

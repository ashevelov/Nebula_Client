namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class GetWorldsOperation : BaseOperationHandler {

        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("GetWorldsOperation: return some error");
            }
            var worldHash = response.Parameters[(byte)ParameterCode.Worlds] as Hashtable;
            game.Engine.GameData.worlds.ParseInfo(worldHash);
        }
    }
}

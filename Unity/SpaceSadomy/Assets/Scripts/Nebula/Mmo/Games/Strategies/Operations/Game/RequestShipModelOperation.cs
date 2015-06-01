using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;
using Nebula.Mmo.Games;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class RequestShipModelOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            NetworkGame.OnShipModelUpdated(game, Result(response));
        }
    }
}

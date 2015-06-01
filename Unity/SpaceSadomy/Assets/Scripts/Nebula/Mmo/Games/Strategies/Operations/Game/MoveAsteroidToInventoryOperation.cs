using Common;
using ExitGames.Client.Photon;
using Game.Network;
using Nebula.Mmo.Games;
using System.Text;
using UnityEngine;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class MoveAsteroidToInventoryOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }
        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
        }
    }
}

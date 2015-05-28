using Common;
using ExitGames.Client.Photon;
using Game.Network;
using System.Text;
using UnityEngine;

namespace Nebula {
    public class WERPCMoveAsteroidToInventoryStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
        }
    }
}

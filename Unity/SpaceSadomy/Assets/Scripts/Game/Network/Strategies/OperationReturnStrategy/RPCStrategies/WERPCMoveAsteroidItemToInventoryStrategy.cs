using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;
using Common;
using Nebula.UI;

namespace Nebula {
    public class WERPCMoveAsteroidItemToInventoryStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
            this.UpdateInventorySourceView(response);

        }
    }
}

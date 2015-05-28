using Common;
using ExitGames.Client.Photon;
using Game.Network;
using UnityEngine;

namespace Nebula
{
    public class WERPCAddFromContainerStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
            
            this.UpdateInventorySourceView(response);
        }
    }
}

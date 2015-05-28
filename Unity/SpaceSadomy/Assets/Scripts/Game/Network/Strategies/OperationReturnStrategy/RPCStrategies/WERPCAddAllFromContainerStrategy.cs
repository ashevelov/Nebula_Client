using Common;
using ExitGames.Client.Photon;
using Game.Network;
using System.Collections.Generic;

namespace Nebula
{
    public class WERPCAddAllFromContainerStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
        }
    }
}

using Common;
using ExitGames.Client.Photon;
using Game.Network;
using UnityEngine;

namespace Nebula
{
    public class WERPCEmptyStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            Debug.Log("Action {0} response received".f(Action(response)));
        }
    }
}

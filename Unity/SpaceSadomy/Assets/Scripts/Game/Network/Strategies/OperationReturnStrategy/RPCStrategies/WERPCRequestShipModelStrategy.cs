using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula {
    public class WERPCRequestShipModelStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            NetworkGame.OnShipModelUpdated(game, Result(response));
        }
    }
}

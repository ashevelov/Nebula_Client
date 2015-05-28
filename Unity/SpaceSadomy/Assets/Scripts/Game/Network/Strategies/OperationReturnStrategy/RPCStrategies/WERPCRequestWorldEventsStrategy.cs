using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula {
    public class WERPCRequestWorldEventsStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            if(Result(response) == null )
            {
                return;
            }
            game.WorldEventConnection.ParseInfo(Result(response));
        }
    }
}

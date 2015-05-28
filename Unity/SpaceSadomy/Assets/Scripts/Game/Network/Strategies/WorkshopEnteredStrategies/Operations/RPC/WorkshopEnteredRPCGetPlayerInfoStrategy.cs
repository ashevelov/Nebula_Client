using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Space;

namespace Nebula {
    public class WorkshopEnteredRPCGetPlayerInfoStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.PlayerInfo.ParseInfo(Result(response));
        }
    }
}

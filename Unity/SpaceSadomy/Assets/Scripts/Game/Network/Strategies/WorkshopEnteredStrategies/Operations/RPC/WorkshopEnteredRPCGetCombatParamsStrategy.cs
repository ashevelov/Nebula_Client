using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula {
    public class WorkshopEnteredRPCGetCombatParamsStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.CombatStats.ParseInfo(Result(response));
        }
    }
}

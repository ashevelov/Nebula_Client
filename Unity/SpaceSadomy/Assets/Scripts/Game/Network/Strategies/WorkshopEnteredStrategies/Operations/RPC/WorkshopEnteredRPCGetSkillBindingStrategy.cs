using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Space;

namespace Nebula {
    public class WorkshopEnteredRPCGetSkillBindingStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.Skills.ParseInfo(Result(response));
        }
    }
}

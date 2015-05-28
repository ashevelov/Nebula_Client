using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula
{
    public class WERPCGetSkillBindingStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.Skills.ParseInfo(Result(response));
        }
    }
}

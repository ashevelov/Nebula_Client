using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Game.Space;
using Game.Network;
using Nebula;

namespace Nebula
{
    public class WERPCGetChatUpdateStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            Debug.Log("chat update received: {0}".f(Result(response).Count));
            Result(response).Print(1);
            game.Chat.ReceiveUpdate(Result(response));
        }
    }
}

using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula {
    public class WERaiseGenericEventStrategy : OperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            base.Handle(game, response);
        }
    }
}
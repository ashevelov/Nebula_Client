using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula {
    public class WEDetachInterestAreaStrategy : OperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            HandleEventInterestAreaDetached(game);
        }

        private void HandleEventInterestAreaDetached(NetworkGame game)
        {
            game.OnCameraDetached();
        }
    }
}
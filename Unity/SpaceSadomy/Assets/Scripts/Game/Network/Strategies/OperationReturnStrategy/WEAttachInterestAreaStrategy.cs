using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Game.Network;

namespace Nebula {
    public class WEAttachInterestAreaStrategy : OperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            this.HandleEventInterestAreaAttached(game, response.Parameters);
        }

        private void HandleEventInterestAreaAttached(NetworkGame game, IDictionary eventData)
        {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            game.OnCameraAttached(itemId, itemType);
        }
    }
}

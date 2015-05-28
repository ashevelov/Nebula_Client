using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Game.Network;

namespace Nebula {
    public class WESpawnItemStrategy : OperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            HandleEventItemSpawned(game, response.Parameters);
        }

        private void HandleEventItemSpawned(NetworkGame game, IDictionary eventData)
        {

            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            game.OnItemSpawned(itemType, itemId);

            Debug.Log(string.Format("item spawned: {0} of type: {1}", itemId, itemType.toItemType()));
        }
    }
}

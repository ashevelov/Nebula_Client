using Common;
using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class SpawnItemOperation : BaseOperationHandler
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleEventItemSpawned((NetworkGame)game, response.Parameters);
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

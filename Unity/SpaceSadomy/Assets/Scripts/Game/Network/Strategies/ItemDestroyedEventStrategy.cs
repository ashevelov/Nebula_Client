namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;


    public class ItemDestroyedEventStrategy : IServerEventStrategy {
        public void Handle(NetworkGame game, EventData eventData) {
            HandleEventItemDestroyed(game, eventData.Parameters);
        }

        private void HandleEventItemDestroyed(NetworkGame game, IDictionary eventData) {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            Item item;
            if (game.TryGetItem(itemType, itemId, out item)) {
                item.IsDestroyed = game.RemoveItem(item);
            }
        }
    } 
}

namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Mmo.Games;

    public class ItemDestroyedEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEventItemDestroyed((NetworkGame)game, eventData.Parameters);
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

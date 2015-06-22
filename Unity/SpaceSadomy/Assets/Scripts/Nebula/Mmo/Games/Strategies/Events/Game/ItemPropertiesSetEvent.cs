namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Mmo.Games;
    using Nebula.Mmo.Items;

    public class ItemPropertiesSetEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEventItemPropertiesSet((NetworkGame)game, eventData.Parameters);
        }


        private void HandleEventItemPropertiesSet(NetworkGame game, IDictionary eventData) {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            Item item;
            if (game.TryGetItem(itemType, itemId, out item)) {
                item.PropertyRevision = (int)eventData[(byte)ParameterCode.PropertiesRevision];

                Hashtable propertiesSet = eventData[(byte)ParameterCode.PropertiesSet] as Hashtable;
                item.SetProperties(propertiesSet);
            }
        }
    }

}
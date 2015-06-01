namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Mmo.Games;

    public class ItemPropertiesEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEventItemProperties((NetworkGame)game, eventData.Parameters);
        }

        private void HandleEventItemProperties(NetworkGame game, IDictionary eventData) {
            //game.Listener.LogInfo(game, "HandleEventItemProperties");
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            Item item;
            if (game.TryGetItem(itemType, itemId, out item)) {
                item.PropertyRevision = (int)eventData[(byte)ParameterCode.PropertiesRevision];

                Hashtable propertiesSet = eventData[(byte)ParameterCode.PropertiesSet] as Hashtable;
                //game.Listener.LogInfo(game, "properties count: " + propertiesSet.Count);

                item.SetProperties(propertiesSet);

            }
        }
    }

}
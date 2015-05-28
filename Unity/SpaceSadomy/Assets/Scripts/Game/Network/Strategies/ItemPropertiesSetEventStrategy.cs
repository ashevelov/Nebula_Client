namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class ItemPropertiesSetEventStrategy : IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            HandleEventItemPropertiesSet(game, eventData.Parameters);
        }

        private void HandleEventItemPropertiesSet(NetworkGame game, IDictionary eventData) {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            Item item;
            if (game.TryGetItem(itemType, itemId, out item)) {
                item.PropertyRevision = (int)eventData[(byte)ParameterCode.PropertiesRevision];

                Hashtable propertiesSet = eventData[(byte)ParameterCode.PropertiesSet] as Hashtable;
                foreach (DictionaryEntry entry in propertiesSet) {
                    Hashtable groupProps = entry.Value as Hashtable;
                    if (groupProps == null) {
                        continue;
                    }
                    item.SetProperties(entry.Key.ToString(), groupProps);
                }
            }
        }
    }

}
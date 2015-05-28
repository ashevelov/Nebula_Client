namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;


    public class ItemPropertiesEventStrategy : IServerEventStrategy {


        public void Handle(NetworkGame game, EventData eventData) {
            HandleEventItemProperties(game, eventData.Parameters);
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
namespace Nebula {
    using UnityEngine;
    using ExitGames.Client.Photon;
    using Common;
    using System.Collections.Generic;
    using Nebula.Client.Inventory;
    using Nebula.UI;

    public class GenericEventInventoryItemsAddedStrategy : GenericEventStrategy, IServerEventStrategy {
        public void Handle(NetworkGame game, EventData eventData) {
            List<ClientInventoryItem> inventoryItems = InventoryObjectInfoFactory.ParseItemsArray(AddedObjects(eventData));
            game.AddToInventoryTransaction(ContainerId(eventData), ContainerType(eventData), inventoryItems);
            Debug.Log("number of added objects: {0}".f(inventoryItems.Count));

            //update inventory if opened
            InventoryView.UpdateView();
            Events.EvtPlayerInventoryUpdated();
        }

        private string ContainerId(EventData eventData) {
            return Properties(eventData).GetValue<string>(GenericEventProps.target_id, string.Empty);
        }

        private byte ContainerType(EventData eventData) {
            return Properties(eventData).GetValue<byte>(GenericEventProps.target_type, (byte)0);
        }

        private object[] AddedObjects(EventData eventData) {
            return Properties(eventData).GetValue<object[]>(GenericEventProps.InventoryItems, new object[] { });
        }
    }

}
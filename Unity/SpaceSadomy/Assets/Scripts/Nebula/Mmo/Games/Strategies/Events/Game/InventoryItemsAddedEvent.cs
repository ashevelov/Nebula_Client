namespace Nebula.Mmo.Games.Strategies.Events.Game  {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Client.Inventory;
    using Nebula.Mmo.Games;
    using Nebula.UI;
    using ServerClientCommon;
    using System.Collections.Generic;
    using UnityEngine;

    public class InventoryItemsAddedEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            List<ClientInventoryItem> inventoryItems = InventoryObjectInfoFactory.ParseItemsArray(AddedObjects(eventData));
            game.Engine.OnAddToInventoryFromCurrentContainer(ContainerId(eventData), ContainerType(eventData), inventoryItems);
            Debug.Log("number of added objects: {0}".f(inventoryItems.Count));

            //update inventory if opened
            InventoryView.UpdateView();

            global::Nebula.Events.EvtPlayerInventoryUpdated();
        }

        private string ContainerId(EventData eventData) {
            return Properties(eventData).GetValue<string>((int)SPC.Target, string.Empty);
        }

        private byte ContainerType(EventData eventData) {
            return Properties(eventData).GetValue<byte>((int)SPC.TargetType, (byte)0);
        }

        private object[] AddedObjects(EventData eventData) {
            return Properties(eventData).GetValue<object[]>((int)SPC.Items, new object[] { });
        }
    }

}
namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Game.Network;
    using Nebula.UI;

    public class GenericEventInventoryUpdatedStrategy : GenericEventStrategy, IServerEventStrategy {
        public void Handle(NetworkGame game, EventData eventData) {
            NetworkGame.OnInventoryUpdated(game, Properties(eventData));

            //update inventory if opened
            InventoryView.UpdateView();
            Events.EvtPlayerInventoryUpdated();
        }
    } 
}

namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using Nebula.UI;
    using UnityEngine;

    public class InventoryUpdatedEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }
        private void HandleEvent(NetworkGame game, EventData eventData) {
            NetworkGame.OnInventoryUpdated(game, Properties(eventData));

            //update inventory if opened
            InventoryView.UpdateView();
            global::Nebula.Events.EvtPlayerInventoryUpdated();
            Debug.Log("inventory update received event".Color("orange"));
        }
    } 
}

namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;

    public class ItemShipDestroyedEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            game.SetShipDestroyed(ItemType(eventData), ItemId(eventData), ShipDestroyed(eventData));
        }

        private bool ShipDestroyed(EventData eventData) {
            return (bool)eventData.Parameters[(byte)ParameterCode.EventData];
        }

        private string ItemId(EventData eventData) {
            return (string)eventData.Parameters[(byte)ParameterCode.ItemId];
        }

        private byte ItemType(EventData eventData) {
            return (byte)eventData.Parameters[(byte)ParameterCode.ItemType];
        }
    }

}
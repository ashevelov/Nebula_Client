using ExitGames.Client.Photon;
using Nebula.Mmo.Games;

namespace Nebula.Mmo.Games.Strategies.Events.Game {
    public class StationHoldUpdatedEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            NetworkGame.OnStationHoldUpdated(game, Properties(eventData));
            global::Nebula.Events.EvtStationUpdated();
        }
    }
}
using ExitGames.Client.Photon;

namespace Nebula {
    public class WorkshopEnteredStationHoldUpdatedGenericEventStrategy : GenericEventStrategy, IServerEventStrategy {
        public void Handle(NetworkGame game, EventData eventData) {
            NetworkGame.OnStationHoldUpdated(game, Properties(eventData));
            Events.EvtStationUpdated();
        }
    }
}
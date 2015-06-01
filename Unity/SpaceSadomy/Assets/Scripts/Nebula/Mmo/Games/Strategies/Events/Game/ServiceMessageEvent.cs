namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;

    public class ServiceMessageEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }
        private void HandleEvent(NetworkGame game, EventData eventData) {
            game.ServiceMessageReceiver.AddMessage(Properties(eventData));
        }
    }

}
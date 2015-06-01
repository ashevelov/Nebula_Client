namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using UnityEngine;

    public class PlayerInfoUpdatedEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            if (Properties(eventData) == null) {
                Debug.Log("Player info properties is null");
                return;
            }
            game.SetPlayerInfo(Properties(eventData));
        }
    } 
}
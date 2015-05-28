namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Game.Network;

    public class WorldExitedEventStrategy : IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            if (game.WorldTransition.HasNextWorld()) {
                game.SetWaitingForChangeWorld();
            } else {
                game.SetSelectCharacter();
            }
        }
    }

}
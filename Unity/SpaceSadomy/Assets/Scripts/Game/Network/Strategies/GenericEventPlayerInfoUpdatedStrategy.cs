namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Game.Network;

    public class GenericEventPlayerInfoUpdatedStrategy : GenericEventStrategy, IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            if (Properties(eventData) == null) {
                Debug.Log("Player info properties is null");
                return;
            }
            game.SetPlayerInfo(Properties(eventData));
        }
    } 
}
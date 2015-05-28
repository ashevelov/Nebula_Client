namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Game.Network;

    public class GenericEventChatMessageStrategy : GenericEventStrategy, IServerEventStrategy {
        public void Handle(NetworkGame game, EventData eventData) {
            Debug.Log("Chat message generic event not implemented");
        }
    }

}
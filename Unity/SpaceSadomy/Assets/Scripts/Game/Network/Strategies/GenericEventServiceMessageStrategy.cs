namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Game.Network;

    public class GenericEventServiceMessageStrategy : GenericEventStrategy, IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            game.ServiceMessageReceiver.AddMessage(Properties(eventData));
        }
    }

}
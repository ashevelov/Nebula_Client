namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Game.Network;

    public class GenericEventShipModelUpdatedStrategy : GenericEventStrategy, IServerEventStrategy {
        public void Handle(NetworkGame game, EventData eventData) {
            NetworkGame.OnShipModelUpdated(game, Properties(eventData));
        }
    }

}
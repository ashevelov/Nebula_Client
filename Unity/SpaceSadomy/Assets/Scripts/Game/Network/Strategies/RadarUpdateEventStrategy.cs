namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Game.Network;

    public class RadarUpdateEventStrategy : IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            HandleEventRadarUpdate(eventData.Parameters, game);
        }

        private void HandleEventRadarUpdate(IDictionary eventData, NetworkGame game) {
        }
    }

}
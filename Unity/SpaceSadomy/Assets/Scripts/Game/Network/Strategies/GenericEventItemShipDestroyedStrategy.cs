namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Game.Network;

    public class GenericEventItemShipDestroyedStrategy : GenericEventStrategy, IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            game.SetShipDestroyed(ItemType(eventData), ItemId(eventData), ShipDestroyed(eventData));
        }

        private bool ShipDestroyed(EventData eventData) {
            return (bool)eventData.Parameters[(byte)ParameterCode.EventData];
        }

        private string ItemId(EventData eventData) {
            return (string)eventData.Parameters[(byte)ParameterCode.ItemId];
        }

        private byte ItemType(EventData eventData) {
            return (byte)eventData.Parameters[(byte)ParameterCode.ItemType];
        }
    }

}
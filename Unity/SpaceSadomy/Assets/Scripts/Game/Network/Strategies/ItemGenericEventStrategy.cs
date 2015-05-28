namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using System.Collections.Generic;
    using Common;
    using Game.Network;

    public class ItemGenericEventStrategy : IServerEventStrategy {
        private Dictionary<CustomEventCode, IServerEventStrategy> strategies;

        public ItemGenericEventStrategy() {
            this.strategies = new Dictionary<CustomEventCode, IServerEventStrategy>();
        }

        public void AddStrategy(CustomEventCode eventCode, IServerEventStrategy strategy) {
            this.strategies[eventCode] = strategy;
        }

        public void RemoveStrategy(CustomEventCode eventCode) {
            this.strategies.Remove(eventCode);
        }

        public void Handle(NetworkGame game, EventData eventData) {
            IServerEventStrategy strategy = null;
            if (!this.strategies.TryGetValue(CustomCode(eventData), out strategy)) {
                Debug.Log("strategy for custom code: {0} not founded".f(CustomCode(eventData)));
                return;
            }
            strategy.Handle(game, eventData);
        }

        private CustomEventCode CustomCode(EventData eventData) {
            return (CustomEventCode)(byte)eventData.Parameters[(byte)ParameterCode.CustomEventCode];
        }
    }

}
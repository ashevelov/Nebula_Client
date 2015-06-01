namespace Nebula.Mmo.Games.Strategies.Events {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using System.Collections.Generic;
    using Common;
    using Nebula.Mmo.Games;

    public class GenericContainerEvent : BaseEventHandler {

        private Dictionary<CustomEventCode, BaseGenericEvent> strategies;

        public GenericContainerEvent() {
            this.strategies = new Dictionary<CustomEventCode, BaseGenericEvent>();
        }

        public override void Handle(BaseGame game, EventData eventData) {
            BaseGenericEvent strategy = null;
            if (!this.strategies.TryGetValue(CustomCode(eventData), out strategy)) {
                Debug.Log("strategy for custom code: {0} not founded".f(CustomCode(eventData)));
                return;
            }
            strategy.Handle(game, eventData);
        }

        public void AddStrategy(CustomEventCode eventCode, BaseGenericEvent strategy) {
            this.strategies[eventCode] = strategy;
        }

        public void RemoveStrategy(CustomEventCode eventCode) {
            this.strategies.Remove(eventCode);
        }

        private CustomEventCode CustomCode(EventData eventData) {
            return (CustomEventCode)(byte)eventData.Parameters[(byte)ParameterCode.CustomEventCode];
        }
    }

}
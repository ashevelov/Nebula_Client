using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using Common;

namespace Nebula.Mmo.Games.Strategies {
    public abstract class DefaultStrategy : IGameStrategy {

        protected Dictionary<byte, BaseOperationHandler> operationHandlers;
        protected Dictionary<byte, BaseEventHandler> eventHandlers;

        public abstract GameState State { get; }

        public DefaultStrategy() {
            operationHandlers = new Dictionary<byte, BaseOperationHandler>();
            eventHandlers = new Dictionary<byte, BaseEventHandler>();
        }

        public virtual void OnEventReceive(BaseGame game, EventData eventData) {
            if(eventHandlers.ContainsKey(eventData.Code)) {
                eventHandlers[eventData.Code].Handle(game, eventData);
            } else {
                Debug.LogFormat("strategy {0} don't contains event handler {1}", State, (SelectCharacterEventCode)eventData.Code);
            }
        }

        public virtual void OnOperationReturn(BaseGame game, OperationResponse operationResponse) {
            if(operationHandlers.ContainsKey(operationResponse.OperationCode)) {
                operationHandlers[operationResponse.OperationCode].Handle(game, operationResponse);
            } else {
                Debug.LogFormat("strategy {0} don't contains operation handler {1}", State, (OperationCode)operationResponse.OperationCode);
            }
        }

        public virtual void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            Debug.LogFormat("strategy {0} peer status changed {1}", State, statusCode);
        }

        public void AddOperationHandler(byte code, BaseOperationHandler handler) {
            operationHandlers.Add(code, handler);
        }

        public void AddEventHandler(byte code, BaseEventHandler handler) {
            eventHandlers.Add(code, handler);
        }
    }
}

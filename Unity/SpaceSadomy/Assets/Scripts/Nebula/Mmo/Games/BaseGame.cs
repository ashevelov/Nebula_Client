using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using Nebula.Mmo.Games.Strategies;
using Common;

namespace Nebula.Mmo.Games {
    public abstract class BaseGame : IPhotonPeerListener {

        private int outgoingOperationCount = 0;

        public PhotonPeer Peer { get; protected set; }
        public MmoEngine Engine { get; private set; }
        public Settings Settings { get; private set; }

        protected Dictionary<GameState, IGameStrategy> strategies;
        protected IGameStrategy activeStrategy;

        public BaseGame(MmoEngine engine, Settings settings) {
            Engine = engine;
            Settings = settings;
        }

        public void SetPeer(PhotonPeer peer) {
            Peer = peer;
        }

        public virtual void DebugReturn(DebugLevel level, string message) {
            Debug.LogFormat("{0} DebugReturn level = {1} message = {2}", GameType, level, message);
        }

        public virtual void OnEvent(EventData eventData) {
            //Debug.LogFormat("{0} OnEvent code = {1}", GameType, eventData.Code);
            activeStrategy.OnEventReceive(this, eventData);
        }

        public virtual void OnOperationResponse(OperationResponse operationResponse) {
            //Debug.LogFormat("{0} OpOperationResponse code = {1}", GameType, operationResponse.OperationCode);
            activeStrategy.OnOperationReturn(this, operationResponse);
        }

        public virtual void OnStatusChanged(StatusCode statusCode) {
            Debug.LogFormat("{0} OnStatusChanged = {1}", GameType, statusCode);
            activeStrategy.OnPeerStatusChanged(this, statusCode);
        }

        public virtual void Connect(string ipAddress, int port, string application) {
            if(Peer != null ) {
                Peer.Connect(ipAddress + ":" + port, application);
            }
        }

        public virtual void Disconnect() {
            if(Peer != null) {
                Peer.Disconnect();
            }
        }

        public virtual void Update() {
            if(Peer != null ) {
                Peer.Service();
            }
        }
        public abstract GameType GameType { get; }

        public virtual GameState CurrentStrategy {
            get {
                return activeStrategy.State;
            }
        }

        public virtual void SetStrategy(GameState state) {
            if (strategies.ContainsKey(state)) {
                activeStrategy = strategies[state];
                Events.EvtGameBehaviourChanged(GameType, CurrentStrategy);
            } else {
                Debug.LogErrorFormat("no strategy for state {0}", state);
            }
        }

        public void SendOperation(byte operationCode, Dictionary<byte, object> parameters, bool sendReliable, byte channelId ) {
            Peer.OpCustom(operationCode, parameters, sendReliable, channelId);
            outgoingOperationCount++;
            if(outgoingOperationCount > Settings.OutgoingOperationCount) {
                Peer.SendOutgoingCommands();
                outgoingOperationCount = 0;
            }
        }

        public void SendOperation(OperationCode operationCode, Dictionary<byte, object> parameters, bool sendReliable, byte channelId) {
            this.SendOperation((byte)operationCode, parameters, sendReliable, channelId);
        }
    }
}

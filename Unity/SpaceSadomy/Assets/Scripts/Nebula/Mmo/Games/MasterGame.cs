using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Nebula.Mmo.Games.Strategies;
using UnityEngine;
using Nebula.Client.Servers;
using ServerClientCommon;

namespace Nebula.Mmo.Games {
    public class MasterGame : BaseGame {

        private ClientServerCollection serverCollection;

        public MasterGame(MmoEngine engine, Settings settings)
            : base(engine, settings) {
            strategies = new Dictionary<GameState, IGameStrategy> {
                {GameState.MasterDisconnected, new MasterDisconnectedStrategy() },
                {GameState.MasterWaitingConnect, new MasterWaitingConnectStrategy() },
                {GameState.MasterConnected, new MasterConnectedStrategy() }
            };
            SetStrategy(GameState.MasterDisconnected);
        }

        public override void Connect(string ipAddress, int port, string application) {
            SetStrategy(GameState.MasterWaitingConnect);
            base.Connect(ipAddress, port, application);
        }
        public override GameType GameType {
            get {
                return GameType.Master;
            }
        }

        public void SetServerCollection(ClientServerCollection serverCollection) {
            this.serverCollection = serverCollection;
        }

        public bool AllServersPresents() {
            if (serverCollection == null) {
                return false;
            }
            bool containsLogin = serverCollection.ContainsServer(ServerType.login);
            bool containsSelectCharacter = serverCollection.ContainsServer(ServerType.character);
            bool containsGame = serverCollection.ContainsServer(ServerType.game);
            return containsLogin && containsSelectCharacter && containsGame;
        }

        public ServerInfo GetServer(ServerType serverType) {
            return serverCollection.GetServer(serverType);
        }

        public ServerInfo GetGameServer(string world) {
            return serverCollection.GetGameServerForLocation(world);
        }

    }


}
namespace Nebula.Mmo.Games
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Collections.Generic;
    using Nebula.Mmo.Games.Strategies;

    public class LoginGame : BaseGame {

        private string gameRefId;

        public LoginGame(MmoEngine engine, Settings settings) 
            : base(engine, settings) {
            strategies = new Dictionary<GameState, IGameStrategy> {
                {GameState.LoginDisconnected, new LoginDisconnectedStrategy() },
                { GameState.LoginWaitingConnect, new LoginWaitingConnectStrategy() },
                { GameState.LoginConnected, new LoginConnectedStrategy() }
            };
            SetStrategy(GameState.LoginDisconnected);
        }

        public override void Connect(string ipAddress, int port, string application) {
            SetStrategy(GameState.LoginWaitingConnect);
            base.Connect(ipAddress, port, application);
        }

        public override GameType GameType {
            get {
                return GameType.Login;
            }
        }

        public void SetGameRefId(string grID) {
            gameRefId = grID;
        }

        public string GameRefId {
            get {
                return gameRefId;
            }
        }

        public bool IsGameRefIdValid() {
            return (false == string.IsNullOrEmpty(GameRefId));
        }
    }
}

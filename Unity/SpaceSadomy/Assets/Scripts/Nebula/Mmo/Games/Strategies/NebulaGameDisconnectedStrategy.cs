namespace Nebula.Mmo.Games.Strategies {
    using UnityEngine;
    using System.Collections;
    using System;
    using ExitGames.Client.Photon;

    public class NebulaGameDisconnectedStrategy : DefaultStrategy {

        public override void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            base.OnPeerStatusChanged(game, statusCode);
            switch(statusCode) {
                case StatusCode.Connect:
                    {
                        game.SetStrategy(GameState.NebulaGameConnected);
                        break;
                    }
            }
        }

        public override GameState State {
            get {
                return GameState.NebulaGameDisconnected;
            }
        }
    }
}

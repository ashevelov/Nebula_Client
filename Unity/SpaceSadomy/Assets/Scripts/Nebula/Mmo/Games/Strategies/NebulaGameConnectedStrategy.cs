namespace Nebula.Mmo.Games.Strategies {
    using UnityEngine;
    using System.Collections;
    using System;
    using ExitGames.Client.Photon;

    public class NebulaGameConnectedStrategy : DefaultStrategy {

        public override GameState State {
            get {
                return GameState.NebulaGameConnected;
            }
        }

        public override void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            switch(statusCode) {
                case StatusCode.Connect:
                    {
                        game.SetStrategy(GameState.NebulaGameConnected);
                        break;
                    }
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
                    {
                        game.SetStrategy(GameState.NebulaGameDisconnected);
                        break;
                    }
                default:
                    {
                        Debug.LogFormat("Unhandled game peer status code {0}", statusCode);
                        break;
                    }
            }
        }
    }
}

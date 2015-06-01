using ExitGames.Client.Photon;
using UnityEngine;

namespace Nebula.Mmo.Games.Strategies {

    public class NebulaGameWaitingConnectStrategy : DefaultStrategy {

        public override GameState State {
            get {
                return GameState.NebulaGameWaitingConnect;
            }
        }

        public override void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            switch (statusCode) {
                case StatusCode.Connect:
                    {
                        Debug.Log("Nebula game peer connect");
                        game.SetStrategy(GameState.NebulaGameConnected);
                        break;
                    }
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
                    {
                        Debug.Log("Nebula game peer disconnect");
                        game.SetStrategy(GameState.NebulaGameDisconnected);
                        break;
                    }
                default:
                    {
                        Debug.LogFormat("Unhandled nebula game peer status code {0}", statusCode);
                        break;
                    }
            }
        }
    }

}
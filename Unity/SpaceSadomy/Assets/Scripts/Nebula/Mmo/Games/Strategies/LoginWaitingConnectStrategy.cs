using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using Nebula.UI;

namespace Nebula.Mmo.Games.Strategies {
    public class LoginWaitingConnectStrategy : DefaultStrategy {
        public override GameState State {
            get {
                return GameState.LoginWaitingConnect;
            }
        }

        public override  void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            switch(statusCode) {
                case StatusCode.Connect:
                    {
                        Debug.Log("Login peer connect");
                        game.SetStrategy(GameState.LoginConnected);
                        MainCanvas.Get.Show(CanvasPanelType.LoginView);
                        //NRPC.Login(game, "123456789", "ABSDEFGH");
                        break;
                    }
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
                    {
                        Debug.Log("Login peer disconnect");
                        game.SetStrategy(GameState.LoginDisconnected);
                        break;
                    }
                default:
                    {
                        Debug.LogFormat("Unhandled login peer status code {0}", statusCode);
                        break;
                    }
            }
        }
    }
}

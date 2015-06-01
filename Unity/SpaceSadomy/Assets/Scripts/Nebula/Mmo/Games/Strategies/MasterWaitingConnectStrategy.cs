using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;

namespace Nebula.Mmo.Games.Strategies {
    public class MasterWaitingConnectStrategy : DefaultStrategy {
        public override GameState State {
            get {
                return GameState.MasterWaitingConnect;
            }
        }


        public override  void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            switch(statusCode) {
                case StatusCode.Connect:
                    {
                        Debug.Log("Master peer connect");
                        game.SetStrategy(GameState.MasterConnected);
                        NRPC.GetServerList(game);
                        break;
                    }
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
                    {
                        Debug.Log("Master peer disconnect");
                        game.SetStrategy(GameState.MasterDisconnected);
                        break;
                    }
                default:
                    {
                        Debug.LogFormat("Unhandled master peer status code {0}", statusCode);
                        break;
                    }
            }
        }
    }
}

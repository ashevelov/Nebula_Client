namespace Nebula.Mmo.Games.Strategies {
    using UnityEngine;
    using System.Collections;
    using System;
    using ExitGames.Client.Photon;

    public class SelectCharacterWaitingConnectStrategy : DefaultStrategy {
        public override GameState State {
            get {
                return GameState.SelectCharacterWaitingConnect;
            }
        }

        public override void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            switch (statusCode) {
                case StatusCode.Connect:
                    {
                        Debug.Log("Select character peer connect");
                        game.SetStrategy(GameState.SelectCharacterConnected);
                        SelectCharacterGame.Instance().RegisterClient();
                        SelectCharacterGame.GetCharacters(game.Engine.LoginGame.GameRefId, game.Engine.LoginGame.login);
                        break;
                    }
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
                    {
                        Debug.Log("Select character peer disconnect");
                        game.SetStrategy(GameState.SelectCharacterDisconnected);
                        break;
                    }
                default:
                    {
                        Debug.LogFormat("Unhandled select character peer status code {0}", statusCode);
                        break;
                    }
            }
        }
    }
}

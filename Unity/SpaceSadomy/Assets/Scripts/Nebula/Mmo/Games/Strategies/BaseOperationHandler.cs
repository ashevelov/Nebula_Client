using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;

namespace Nebula.Mmo.Games.Strategies {
    public class BaseOperationHandler {

        public virtual void Handle(BaseGame game, OperationResponse response) {
            Debug.LogFormat("Handler at game {0} for operation {1}", game.GameType, response.OperationCode);
        }

        protected bool CheckSuccess(BaseGame game, OperationResponse response) {
            if(response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogWarningFormat("Game = {0}, Operation = {1}, error = {2}, message = {3}",
                    game.GameType, response.OperationCode, (ReturnCode)response.ReturnCode, response.DebugMessage);
                return false;
            }
            return true;
        }
    }
}

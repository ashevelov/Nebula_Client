using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;

namespace Nebula.Mmo.Games.Strategies.Operations.Login {
    public class LoginOperation : BaseOperationHandler {

        public override void Handle(BaseGame game, OperationResponse response) {
            if(response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogErrorFormat("error, return code = {0}, debug message = {1}", (ReturnCode)response.ReturnCode, response.DebugMessage);
                return;
            }

            string gameRefId = (string)response.Parameters[(byte)ParameterCode.GameRefId];
            string login = (string)response.Parameters[(byte)ParameterCode.Login];

            game.Engine.OnGameRefIdReceived(gameRefId, login);
        }
    }
}

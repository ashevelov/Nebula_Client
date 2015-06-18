namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    using Common;

    public class InvokeMethodOperation : BaseOperationHandler {

        private readonly Dictionary<string, System.Action<SelectCharacterGame, OperationResponse>> mHandlers;

        public InvokeMethodOperation() {
            mHandlers = new Dictionary<string, System.Action<SelectCharacterGame, OperationResponse>>();
            mHandlers.Add("SetYesNoNotification", HandleSetYesNoNotification);
        }

        public override void Handle(BaseGame game, OperationResponse response) {
            string methodName = (string)response.Parameters[(byte)ParameterCode.Action];
            SelectCharacterGame scgame = game as SelectCharacterGame;
            if(mHandlers.ContainsKey(methodName)) {
                mHandlers[methodName](scgame, response);
            }
        }

        private void HandleSetYesNoNotification(SelectCharacterGame game, OperationResponse response) {
            Debug.LogFormat("SetYesNoNotification invoke completed with reurn = {0}", ReturnValue<int>(response));
        }

        private T ReturnValue<T>(OperationResponse response) {
            return (T)response.Parameters[(byte)ParameterCode.Result];
        }
    }
}

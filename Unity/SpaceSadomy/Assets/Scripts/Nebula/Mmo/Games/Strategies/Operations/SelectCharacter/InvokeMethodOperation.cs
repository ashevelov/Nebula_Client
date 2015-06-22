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
            mHandlers.Add("FindGuilds", HandleFindGuilds);
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

        private void HandleFindGuilds(SelectCharacterGame game, OperationResponse response) {
            Hashtable guilds = ReturnValue<Hashtable>(response);
            if(guilds == null ) {
                Debug.Log("return null guilds");
                return;
            }
            Debug.LogFormat("found {0} guilds", guilds.Count);
        }

        private T ReturnValue<T>(OperationResponse response) {
            return (T)response.Parameters[(byte)ParameterCode.Result];
        }
    }
}

namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Client;

    public class SelectCharacterOperation : BaseOperationHandler {

        public override void Handle(BaseGame game, OperationResponse response) {
            if (response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogException(new NebulaException(string.Format(
                    "Error at operation = {0}, return code = {1}, debug message = {2}",
                    (SelectCharacterOperationCode)response.OperationCode,
                    (ReturnCode)response.ReturnCode, response.DebugMessage
                    )));
                return;
            }

            string characterID = (string)response.Parameters[(byte)ParameterCode.CharacterId];
            Hashtable characters = (Hashtable)response.Parameters[(byte)ParameterCode.Characters];
            game.Engine.OnPlayerCharactersReceived(new ClientPlayerCharactersContainer(characters));
            Debug.Log(string.Format("character selected = {0}", characterID).Color("green"));
        }
    }
}

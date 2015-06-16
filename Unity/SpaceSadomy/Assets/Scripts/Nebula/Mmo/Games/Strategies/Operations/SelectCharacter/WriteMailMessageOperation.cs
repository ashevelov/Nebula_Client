namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Nebula.Client;
    using Common;

    public class WriteMailMessageOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if (response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogException(new NebulaException(string.Format(
                    "Error at operation = {0}, return code = {1}, debug message = {2}",
                    (SelectCharacterOperationCode)response.OperationCode,
                    (ReturnCode)response.ReturnCode, response.DebugMessage
                    )));
                return;
            }
            Debug.Log("message written successfully");
        }
    }
}

namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Client;
    using UnityEngine;

    public class DeleteMailMessageOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if (response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogException(new NebulaException(string.Format(
                    "Error at operation = {0}, return code = {1}, debug message = {2}",
                    (SelectCharacterOperationCode)response.OperationCode,
                    (ReturnCode)response.ReturnCode, response.DebugMessage
                    )));
                return;
            }
            Debug.Log("successfully deleted mail message");
            SelectCharacterGame.Instance().GetMails();
        }
    }
}

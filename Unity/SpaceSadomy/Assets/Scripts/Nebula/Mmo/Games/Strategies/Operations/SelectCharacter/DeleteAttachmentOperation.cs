namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Nebula.Client;
    using Common;

    public class DeleteAttachmentOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if (response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogException(new NebulaException(string.Format(
                    "Error at operation = {0}, return code = {1}, debug message = {2}",
                    (SelectCharacterOperationCode)response.OperationCode,
                    (ReturnCode)response.ReturnCode, response.DebugMessage
                    )));
                return;
            }

            Debug.Log("successfully removed attachment");

            Hashtable objectInfo = response.Parameters[(byte)ParameterCode.Info] as Hashtable;

            Debug.Log("trying add attchment to inventory");
            NRPC.AddInventoryItem(objectInfo);
            SelectCharacterGame.Instance().GetMails();
        }
    }
}

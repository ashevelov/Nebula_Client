using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Nebula.Mmo.Games;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class EnterWorkshopOperation : BaseOperationHandler
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            Debug.Log("item {0} workshop entered response".f(ItemId(response)));
        }

        private string ItemId(OperationResponse response)
        {
            return (string)response.Parameters[ParameterCode.ItemId.toByte()];
        }

        private Hashtable Info(OperationResponse response)
        {
            return (Hashtable)response.Parameters[ParameterCode.Info.toByte()];
        }
    }

}
using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;

namespace Nebula {
    public class WorkshopEnteredEnterWorkshopOperation : OperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
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
using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Space;
using Common;

namespace Nebula {
    public class WorkshopEnteredRPCEquipModuleStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            Debug.Log("RPC:EquipModule() response");

            if(Status(response) == ACTION_RESULT.FAIL)
            {
                if(Message(response) != "EM0020" )
                {
                    PrintRPCError(response);
                }
                else
                {
                    Hashtable var2replace = new Hashtable
                    {
                        {"count", Result(response)["count"] }
                    };
                    PrintRPCError(response, var2replace);
                }
            }
        }
    }
}

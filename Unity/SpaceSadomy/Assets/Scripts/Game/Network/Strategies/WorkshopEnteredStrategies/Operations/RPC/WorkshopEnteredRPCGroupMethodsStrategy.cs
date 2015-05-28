using UnityEngine;
using System.Collections;

using ExitGames.Client.Photon;
using Common;
using System.Collections.Generic;

namespace Nebula {
    public class WorkshopEnteredRPCGroupMethodsStrategy : RPCOperationReturnStrategy
    {
        private readonly Dictionary<string, System.Action<NetworkGame, OperationResponse>> handlers;


        public WorkshopEnteredRPCGroupMethodsStrategy()
        {
            this.handlers = new Dictionary<string, System.Action<NetworkGame, OperationResponse>>();
            this.handlers.Add("MoveItemFromInventoryToStation", HandlerMoveItemFromInventoryToStation);
            this.handlers.Add("MoveItemFromStationToInventory", HandlerMoveItemFromStationToInventory);
            this.handlers.Add("TransformObjectAndMoveToHold", HandleTransformObjectAndMoveToHold);
            this.handlers.Add("AddInventorySlots", WERPCGroupMethodsStrategy.HandleAddInventorySlots);
            this.handlers.Add("FreeGroup", WERPCGroupMethodsStrategy.HandleFreeGroup);
            this.handlers.Add("SetLeaderToCharacter", WERPCGroupMethodsStrategy.HandleSetLeaderToCharacter);
            this.handlers.Add("VoteForExclude", WERPCGroupMethodsStrategy.HandleVoteForExclude);
            this.handlers.Add("SetGroupOpened", WERPCGroupMethodsStrategy.HandleSetGroupOpened);
            this.handlers.Add("RequestOpenedGroups", WERPCGroupMethodsStrategy.HandleRequestOpenedGroups);
            this.handlers.Add("JoinToOpenedGroup", WERPCGroupMethodsStrategy.HandleJoinToOpenedGroup);
            this.handlers.Add("CraftItEasy", HandleCraftItEasy);
        }


        public override void Handle(NetworkGame game, OperationResponse response)
        {
            if(this.handlers.ContainsKey(Action(response)))
            {
                this.handlers[Action(response)](game, response);
            }
        }

        private void HandlerMoveItemFromInventoryToStation(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
        }

        private void HandlerMoveItemFromStationToInventory(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
        }

        public static void HandleTransformObjectAndMoveToHold(NetworkGame game, OperationResponse response) {

            if(ResponseStatus(response) == ACTION_RESULT.SUCCESS ) {
                string itemId = (string)ResponseReturn(response);
                Events.EvtObjectTransformedAndMovedToHold(itemId);
            }
        }

        private void HandleCraftItEasy(NetworkGame game, OperationResponse response) {
            Debug.Log("CraftItEasy() response");
        }

        //private string Action(OperationResponse response)
        //{
        //    return (string)response[(byte)ParameterCode.Action];
        //}


    }
}
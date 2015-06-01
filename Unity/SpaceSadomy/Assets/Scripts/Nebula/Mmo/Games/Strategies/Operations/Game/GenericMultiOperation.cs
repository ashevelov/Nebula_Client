using Common;
using ExitGames.Client.Photon;
using Nebula.UI;
using ServerClientCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GenericMultiOperation : BaseGenericOperation {
        private readonly Dictionary<string, System.Action<NetworkGame, OperationResponse>> handlers;

        public GenericMultiOperation() {
            this.handlers = new Dictionary<string, System.Action<NetworkGame, OperationResponse>>();
            this.handlers.Add("TakeMailAttachment", HandleTakeMailAttachment);
            this.handlers.Add("RemoveMailMessage", HandleRemoveMailMessage);
            this.handlers.Add("SetTarget", this.HandleSetTarget);
            this.handlers.Add("TestBuffs", this.HandleTestBuffs);
            this.handlers.Add("ToggleGodMode", this.HandleToggleGodMode);
            this.handlers.Add("AddScheme", this.HandleAddScheme);
            this.handlers.Add("DestroyInventoryItem", this.HadleDestroyInventoryItem);
            this.handlers.Add("AddInventorySlots", HandleAddInventorySlots);
            this.handlers.Add("SendInviteToGroup", HandleSendInviteToGroup);
            this.handlers.Add("ResponseToGroupRequest", HandleResponseToGroupRequest);
            this.handlers.Add("ExitFromCurrentGroup", HandleExitFromCurrentGroup);
            this.handlers.Add("FreeGroup", HandleFreeGroup);
            this.handlers.Add("SetLeaderToCharacter", HandleSetLeaderToCharacter);
            this.handlers.Add("VoteForExclude", HandleVoteForExclude);
            this.handlers.Add("SetGroupOpened", HandleSetGroupOpened);
            this.handlers.Add("RequestOpenedGroups", HandleRequestOpenedGroups);
            this.handlers.Add("JoinToOpenedGroup", HandleJoinToOpenedGroup);
            this.handlers.Add("MoveItemFromInventoryToStation", HandlerMoveItemFromInventoryToStation);
            this.handlers.Add("MoveItemFromStationToInventory", HandlerMoveItemFromStationToInventory);
            this.handlers.Add("TransformObjectAndMoveToHold", HandleTransformObjectAndMoveToHold);
            this.handlers.Add("CraftItEasy", HandleCraftItEasy);
        }

        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }
        private void HandleOperation(NetworkGame game, OperationResponse response) {
            if (this.handlers.ContainsKey(Action(response))) {
                this.handlers[Action(response)](game, response);
            }
        }

        private void HandlerMoveItemFromInventoryToStation(NetworkGame game, OperationResponse response) {
            PrintRPCError(response);
        }

        private void HandlerMoveItemFromStationToInventory(NetworkGame game, OperationResponse response) {
            PrintRPCError(response);
        }

        private void HandleCraftItEasy(NetworkGame game, OperationResponse response) {
            Debug.Log("CraftItEasy() response");
        }

        public static void HandleTransformObjectAndMoveToHold(NetworkGame game, OperationResponse response) {

            if (ResponseStatus(response) == ACTION_RESULT.SUCCESS) {
                string itemId = (string)ResponseReturn(response);
                global::Nebula.Events.EvtObjectTransformedAndMovedToHold(itemId);
            }
        }

        private void HandleTakeMailAttachment(NetworkGame game, OperationResponse response) {
            if (Status(response) == ACTION_RESULT.SUCCESS) {
                Debug.Log("Take attachment: success");
            } else {
                PrintErrorWithErrorMessageId(response);
            }
        }

        private void HandleRemoveMailMessage(NetworkGame game, OperationResponse response) {
            if (Status(response) == ACTION_RESULT.SUCCESS) {
                Debug.Log("Message successfully deleted");
            } else {
                PrintErrorWithErrorMessageId(response);
            }
        }

        private void HandleSetTarget(NetworkGame game, OperationResponse response) {
            if(Status(response) == ACTION_RESULT.SUCCESS ) {
                Hashtable targetInfo = this.Return(response) as Hashtable;
                if(targetInfo == null ) {
                    return;
                }
                bool hasTarget = targetInfo.GetValue<bool>((byte)PS.HasTarget, false);
                string targetId = targetInfo.GetValue<string>((byte)PS.TargetId, string.Empty);
                byte targetType = targetInfo.GetValue<byte>((byte)PS.TargetType, ItemType.Avatar.toByte());
                if( G.PlayerItem == null ) {
                    return;
                }
                G.PlayerItem.Target.SetTarget(hasTarget, targetId, targetType);

                if(G.PlayerItem.Target.HasTarget ) {
                    var targetItem = G.PlayerItem.Target.Item;

                    if( (targetItem != null ) && ( targetItem is Nebula.UI.ISelectedObjectContextMenuViewSource) ) {
                        MainCanvas.Get.Show(CanvasPanelType.SelectedObjectContextMenuView, (targetItem as Nebula.UI.ISelectedObjectContextMenuViewSource).ContextViewData());
                    } else {
                        MainCanvas.Get.Destroy(CanvasPanelType.SelectedObjectContextMenuView);
                    }
                } else {
                    MainCanvas.Get.Destroy(CanvasPanelType.SelectedObjectContextMenuView);
                }
            }
        }

        private void HandleTestBuffs(NetworkGame game, OperationResponse response) {
            if(Status(response) == ACTION_RESULT.SUCCESS) {
                Debug.LogFormat("HandleTestBuffs(): {0}", Message(response));
                Debug.LogFormat("HandleTestBuffs(): {0}", (Return(response) as Hashtable).ToHashString());
            } else {
                Debug.LogErrorFormat("HandleTestBuffs(): not success result: {0}", Status(response));
            }
        }

        private void HandleToggleGodMode(NetworkGame game, OperationResponse response) {
            Hashtable retHash = this.Return(response) as Hashtable;
            bool isGod = retHash.GetBool((int)SPC.IsGod);
            G.Game.Engine.GameData.Chat.PastLocalMessage("Gode Mode Turned: {0}".f(isGod));
        }

        //Handler for test RPC 'AddScheme()'
        private void HandleAddScheme(NetworkGame game, OperationResponse response) {

            if(Status(response) == ACTION_RESULT.FAIL) {
                PrintErrorWithErrorMessageId(response);
                return;
            }

            string schemeId = (string)Return(response);
            G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("Scheme added: {0}", schemeId));
        }

        private void HadleDestroyInventoryItem(NetworkGame game, OperationResponse response ) {
            G.Game.Engine.GameData.Chat.PastLocalMessage(Message(response).Color(Color.green));
        }

        private void HandleSendInviteToGroup(NetworkGame game, OperationResponse response ) {

            G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("Invite sended with status: {0}, error code: {1}",
                Status(response), (LogicErrorCode)MessageInt(response)));
        }

        private void HandleResponseToGroupRequest(NetworkGame game, OperationResponse response) {

            if(Status(response) == ACTION_RESULT.SUCCESS) {
                Debug.Log("response to group request success");
            } else {
                LogicErrorCode errorCode = (LogicErrorCode)MessageInt(response);
                Debug.LogErrorFormat("Error of handle response to group request: {0}", errorCode);
            }
        }

        private void HandleExitFromCurrentGroup(NetworkGame game, OperationResponse response) {
            G.Game.Engine.GameData.Chat.PastLocalMessage("Exited from group");
        }

        public static void HandleAddInventorySlots(NetworkGame game, OperationResponse response) {
            if(ResponseStatus(response) == ACTION_RESULT.SUCCESS) {
                int newSlots = (int)ResponseReturn(response);
                G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("Max inventory slots setted to {0}", newSlots));
            } else {
                G.Game.Engine.GameData.Chat.PastLocalMessage("AddInventorySlots: some error occured");
            }
        }

        public static void HandleFreeGroup(NetworkGame game, OperationResponse response ) {
            if(IsSuccessStatus(response)) {
                G.Game.Engine.GameData.Chat.PastLocalMessage("Successfully free group");
            } else {
                G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("Error of free group: {0}", (LogicErrorCode)ResponseMessageInt(response)));
            }
        }

        public static void HandleSetLeaderToCharacter(NetworkGame game, OperationResponse response) {
            if(IsSuccessStatus(response)) {
                G.Game.Engine.GameData.Chat.PastLocalMessage("Successfully changed group leader");
            } else {
                G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("Error of changing group leader: {0}", (LogicErrorCode)ResponseMessageInt(response)));
            }
        }

        public static void HandleVoteForExclude(NetworkGame game, OperationResponse response ) {
            G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("VoteForExclude response code: {0}", (LogicErrorCode)ResponseMessageInt(response)));
        }

        public static void HandleSetGroupOpened(NetworkGame game, OperationResponse response) {

            LogicErrorCode errorCode = (LogicErrorCode)ResponseMessageInt(response);
            if( errorCode != LogicErrorCode.OK) {
                G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("Error of changing group visibilty: {0}", errorCode));
            } else {
                G.Game.Engine.GameData.Chat.PastLocalMessage("Group visibility changed");
            }
        }

        public static void HandleRequestOpenedGroups(NetworkGame game, OperationResponse response) {
            Hashtable serachGroups = response.Parameters[(byte)ParameterCode.Result] as Hashtable;
            G.Game.SearchGroupsResult().ParseInfo(serachGroups);
            global::Nebula.Events.EvtSearchGroupResultUpdated();
        }

        public static void HandleJoinToOpenedGroup(NetworkGame game, OperationResponse response) {
            LogicErrorCode errorCode = (LogicErrorCode)ResponseMessageInt(response);
            G.Game.Engine.GameData.Chat.PastLocalMessage(string.Format("Join to opened group: {0}", errorCode));
        }
    }
}

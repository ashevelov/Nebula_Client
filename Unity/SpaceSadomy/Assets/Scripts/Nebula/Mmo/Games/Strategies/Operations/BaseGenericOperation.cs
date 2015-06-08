namespace Nebula.Mmo.Games.Strategies.Operations {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using ServerClientCommon;
    using Nebula.UI;

    public class BaseGenericOperation :BaseOperationHandler {

        private readonly StringSubCache<string> strings = new StringSubCache<string>();

        public string Action(OperationResponse response) {
            return (string)response[(byte)ParameterCode.Action];
        }

        public Hashtable Result(OperationResponse response) {
            return ResponseResult(response);
        }

        public static Hashtable ResponseResult(OperationResponse response) {
            return (Hashtable)response[(byte)ParameterCode.Result];
        }

        public object Return(OperationResponse response) {
            return ResponseReturn(response);
        }

        public static object ResponseReturn(OperationResponse response) {
            Hashtable funcRet = response[(byte)ParameterCode.Result] as Hashtable;
            if (funcRet != null) {
                if (funcRet.ContainsKey(ACTION_RESULT.RETURN)) {
                    return funcRet[ACTION_RESULT.RETURN];
                }
            }
            return null;
        }

        protected string Status(OperationResponse response) {
            return ResponseStatus(response);
        }

        public static string ResponseStatus(OperationResponse response) {
            return ResponseResult(response).GetValue<string>(ACTION_RESULT.RESULT, string.Empty);
        }

        public static bool IsSuccessStatus(OperationResponse response) {
            return ResponseStatus(response) == ACTION_RESULT.SUCCESS;
        }

        protected string Message(OperationResponse response) {
            return ResponseMessage(response);
        }

        protected int MessageInt(OperationResponse response) {
            return ResponseMessageInt(response);
        }

        public static string ResponseMessage(OperationResponse response) {
            return ResponseResult(response).GetValue<string>(ACTION_RESULT.MESSAGE, string.Empty);
        }

        public static int ResponseMessageInt(OperationResponse response) {
            return ResponseResult(response).GetValue<int>(ACTION_RESULT.MESSAGE, -1);
        }

        protected string ErrorMessageId(OperationResponse response) {
            return Result(response).GetValueOrDefault<string>((int)SPC.ErrorMessageId);
        }

        protected void PrintRPCError(OperationResponse response) {
            if (Status(response) == ACTION_RESULT.FAIL) {
                if (string.IsNullOrEmpty(Message(response))) {
                    return;
                }

                string errorMessage = strings.String(Message(response), Message(response));
                G.Game.Engine.GameData.Chat.PastLocalMessage(errorMessage);
            }
        }

        protected void PrintRPCError(OperationResponse response, Hashtable variablesToReplace) {
            if (Status(response) == ACTION_RESULT.FAIL) {
                if (string.IsNullOrEmpty(Message(response))) {
                    return;
                }

                string errorMessage = strings.String(Message(response), Message(response));

                if (variablesToReplace != null && variablesToReplace.Count > 0) {
                    errorMessage = errorMessage.ReplaceVariables(variablesToReplace);
                }
                G.Game.Engine.GameData.Chat.PastLocalMessage(errorMessage);
            }
        }

        protected void PrintErrorWithErrorMessageId(OperationResponse response) {
            if (Status(response) == ACTION_RESULT.FAIL) {
                if (string.IsNullOrEmpty(ErrorMessageId(response))) { return; }
                string errorMessage = strings.String(ErrorMessageId(response), ErrorMessageId(response));
                G.Game.Engine.GameData.Chat.PastLocalMessage(errorMessage);
            }
        }

        public void UpdateInventorySourceView(OperationResponse response) {
            if (Status(response) == ACTION_RESULT.SUCCESS) {
                Hashtable retHash = Return(response) as Hashtable;

                //check that result exists
                if (retHash != null) {
                    //get container id, type, item id
                    string containerId = retHash.GetValue<string>((int)SPC.ContainerId, string.Empty);
                    byte containerType = retHash.GetValue<byte>((int)SPC.ContainerType, ItemType.Avatar.toByte());
                    string containerContentItemId = retHash.GetValue<string>((int)SPC.ContainerItemId, string.Empty);

                    //check that return  values valid
                    if ((!string.IsNullOrEmpty(containerId)) && (containerType != (byte)ItemType.Avatar) && (!string.IsNullOrEmpty(containerContentItemId))) {
                        //try get container item
                        Item containerItem = null;
                        if (G.Game.TryGetItem(containerType, containerId, out containerItem)) {
                            //check that container item realize interface IInventoryItemsSource
                            if (containerItem is IInventoryItemsSource) {
                                //remove item from container list
                                if (!(containerItem as IInventoryItemsSource).RemoveItem(containerContentItemId)) {
                                    Debug.LogErrorFormat("Error of removing from container {0}:{1} item {2}", containerId, (ItemType)containerType, containerContentItemId);
                                }
                            }
                        }
                    }
                }

                //if exists container view update this
                if (!MainCanvas.Get) {
                    return;
                }
                if (!MainCanvas.Get.Exists(CanvasPanelType.InventorySourceView)) {
                    return;
                }
                MainCanvas.Get.GetView(CanvasPanelType.InventorySourceView).GetComponentInChildren<InventorySourceView>().UpdateInventory();
            }
        }
    }
}

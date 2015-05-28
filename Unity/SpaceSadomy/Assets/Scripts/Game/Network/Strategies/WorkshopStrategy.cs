
using Common;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula {

    public class WorkshopStrategy : IGameLogicStrategy {
        public static readonly WorkshopStrategy Instance = new WorkshopStrategy();
        private readonly Dictionary<EventCode, IServerEventStrategy> eventStrategies;
        private readonly Dictionary<OperationCode, OperationReturnStrategy> operationStrategies;

        public WorkshopStrategy() {
            this.eventStrategies = new Dictionary<EventCode, IServerEventStrategy>();
            this.eventStrategies.Add(EventCode.WorkshopEntered, new WorkshopEnteredEventStrategy());
            this.eventStrategies.Add(EventCode.WorkshopExited, new WorkshopEnteredWorkshopExitedEventStrategy());

            ItemGenericEventStrategy genericEventStrategy = new ItemGenericEventStrategy();
            genericEventStrategy.AddStrategy(CustomEventCode.ShipModelUpdated, new WorkshopEnteredShipModelUpdatedItemGenericEventStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.StationHoldUpdated, new WorkshopEnteredStationHoldUpdatedGenericEventStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.InventoryUpdated, new WorkshopEnteredInventoryUpdatedGenericEventStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.PlayerInfoUpdated, new WorkshopEnteredPlayerInfoUpdatedGenericEventStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.SkillsUpdated, new WorkshopEnteredSkillsUpdatedGenericEventStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.InventoryItemsAdded, new GenericEventInventoryItemsAddedStrategy());
            this.eventStrategies.Add(EventCode.ItemGeneric, genericEventStrategy);

            this.operationStrategies = new Dictionary<OperationCode, OperationReturnStrategy>();
            this.operationStrategies.Add(OperationCode.EnterWorkshop, new WorkshopEnteredEnterWorkshopOperation());

            WERPCActionStrategy rpcStrategy = new WERPCActionStrategy();
            rpcStrategy.AddRPCActionStrategy("GetStation", new WorkshopEnteredRPCGetStationStrategy());
            rpcStrategy.AddRPCActionStrategy("GetWeapon", new WorkshopEnteredRPCGetWeaponStrategy());
            rpcStrategy.AddRPCActionStrategy("GetInventory", new WorkshopEnteredRPCGetInventoryStrategy());
            rpcStrategy.AddRPCActionStrategy("GetPlayerInfo", new WorkshopEnteredRPCGetPlayerInfoStrategy());
            rpcStrategy.AddRPCActionStrategy("GetSkillBinding", new WorkshopEnteredRPCGetSkillBindingStrategy());
            rpcStrategy.AddRPCActionStrategy("GetCombatParams", new WorkshopEnteredRPCGetCombatParamsStrategy());

            WorkshopEnteredRPCGroupMethodsStrategy groupRPCStrategy = new WorkshopEnteredRPCGroupMethodsStrategy();
            rpcStrategy.AddRPCActionStrategy("MoveItemFromInventoryToStation", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("MoveItemFromStationToInventory", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("TransformObjectAndMoveToHold", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("AddInventorySlots", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("FreeGroup", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("SetLeaderToCharacter", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("VoteForExclude", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("SetGroupOpened", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("RequestOpenedGroups", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("JoinToOpenedGroup", groupRPCStrategy);
            rpcStrategy.AddRPCActionStrategy("CraftItEasy", groupRPCStrategy);

            this.operationStrategies.Add(OperationCode.ExecAction, rpcStrategy);

        }


        public GameState State {
            get { return GameState.Workshop; }
        }

        public void OnEventReceive(NetworkGame game, EventData eventData) {
            IServerEventStrategy strategy = null;
            if (this.eventStrategies.TryGetValue((EventCode)eventData.Code, out strategy)) {
                strategy.Handle(game, eventData);
            } else {
                Debug.LogWarning("Unhandled event in WorldEntered: {0}".f((EventCode)eventData.Code));
            }

            #region Refactored Code
            /*
            switch (eventData.Code.toEnum<EventCode>())
            {
                case EventCode.WorkshopEntered:
                    {
                        Hashtable info = eventData.Parameters[ParameterCode.Info.toByte()] as Hashtable;
                        WorkshopStrategyType workshopStrategyType = (WorkshopStrategyType)(byte)eventData.Parameters[ParameterCode.Type.toByte()];

                        if (info != null)
                        {
                            Dbg.Print("station received...");
                            info.Print(1);
                            game.Station.LoadInfo(info);
                        }
                        G.UI.SpaceObjectGuiCollection.Clear();
                        if (game.Avatar != null && game.Avatar.ExistsView)
                        {
                            game.Avatar.DestroyView();
                        }
                        game.ClearItemCache();
                        game.DeleteAvatar();
                        game.ClearCameras();

                        if (workshopStrategyType == WorkshopStrategyType.Angar)
                            Application.LoadLevel("Angar");
                        else if (workshopStrategyType == WorkshopStrategyType.Planet)
                        {
                            if (G.Game.ClientWorld.Zone.Id == "E7")
                            {
                                Application.LoadLevel("Demo - Colorized Red 1");
                            }
                            else
                            {
                                Application.LoadLevel("Demo - Colorized Red");
                            }
                        }
                    }
                    break;
                case EventCode.WorkshopExited:
                    {
                        Debug.Log("workshop exited received".Color(Color.green));

                        float[] pos = eventData.Parameters[ParameterCode.Position.toByte()] as float[];
                        if (HangarShip.Get)
                        {
                            HangarShip.Get.Stop();
                        }
                        game.OnWorkshopExited(pos);
                    }
                    break;
                case EventCode.ItemGeneric:
                    {
                        var customCode = (CustomEventCode)eventData.Parameters[ParameterCode.CustomEventCode.toByte()];
                        switch (customCode)
                        {
                            case CustomEventCode.ShipModelUpdated:
                                {
                                    Hashtable result = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    Dbg.PrintGenericEvent(CustomEventCode.ShipModelUpdated, result);
                                    NetworkGame.OnShipModelUpdated(game, result);
                                }
                                break;
                            case CustomEventCode.StationHoldUpdated:
                                {
                                    Hashtable result = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    NetworkGame.OnStationHoldUpdated(game, result);
                                    Debug.Log("Number items in hold: {0}, number items in station inventory: {1}".f(G.Game.Station.Hold.ItemCount(StationHoldableObjectType.Module), G.Game.Station.StationInventory.SlotsUsed));
                                }
                                break;
                            case CustomEventCode.InventoryUpdated:
                                {
                                    Hashtable result = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    Dbg.PrintGenericEvent(CustomEventCode.InventoryUpdated, result);
                                    NetworkGame.OnInventoryUpdated(game, result);
                                }
                                break;
                            case CustomEventCode.PlayerInfoUpdated:
                                {
                                    //when updated player info on server, sended generic event to client and parsed here
                                    Hashtable info = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    if (info != null)
                                    {
                                        MmoEngine.Get.Game.PlayerInfo.ParseInfo(info);
                                    }
                                }
                                break;
                            case CustomEventCode.SkillsUpdated:
                                {
                                    Hashtable info = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    if (info != null)
                                    {
                                        //MmoEngine.Get.Game.Ship.Skills.ParseInfo(info);
                                        G.Game.Skills.ParseInfo(info);
                                    }
                                }
                                break;

                        }
                    }
                    break;
            }*/

            #endregion
        }

        public void OnOperationReturn(NetworkGame game, OperationResponse operationResponse) {
            if (operationResponse.ReturnCode != (int)ReturnCode.Ok) {
                Debug.LogError((ReturnCode)operationResponse.ReturnCode + " : " + operationResponse.DebugMessage + " : " + ((OperationCode)operationResponse.OperationCode).ToString());
                game.OnUnexpectedOperationError(operationResponse);
                return;
            }

            OperationCode code = (OperationCode)operationResponse.OperationCode;
            if (this.operationStrategies.ContainsKey(code)) {
                this.operationStrategies[code].Handle(game, operationResponse);
            }

            #region Refactored Code
            /*
            switch (operationResponse.OperationCode.toEnum<OperationCode>())
            {
                case OperationCode.EnterWorkshop:
                    {
                        string itemId = (string)operationResponse.Parameters[ParameterCode.ItemId.toByte()];
                        string.Format("enter workshop response for item: {0}", itemId).Bold().Print();
                        Hashtable info = (Hashtable)operationResponse.Parameters[ParameterCode.Info.toByte()];
                        WorkshopStrategyType workshopStrategyType = (WorkshopStrategyType)(byte)operationResponse.Parameters[ParameterCode.Type.toByte()];
                        if (info != null)
                        {
                            info.Print(1);
                        }
                    }
                    break;
                case OperationCode.ExecAction:
                    {
                        string action = (string)operationResponse[ParameterCode.Action.toByte()];
                        Hashtable result = (Hashtable)operationResponse[ParameterCode.Result.toByte()];
                        switch (action)
                        {
                            //when we equipped module on ship model
                            case "EquipModule":
                                {
                                    Dbg.PrintAction(action, result);
                                    //UIManager.Get.ActionResult.SetActionResult(result);
                                }
                                break;
                            //when we transform object from inventory => station hold
                            case "TransformObjectAndMoveToHold":
                                {
                                    Dbg.PrintAction(action, result);
                                    //UIManager.Get.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "GetStation":
                                {
                                    Dbg.PrintAction(action, result);
                                    //UIManager.Get.ActionResult.SetActionResult(result);
                                    G.Game.Station.LoadInfo(result);
                                }
                                break;
                            case "AddScheme":
                                {
                                    Dbg.PrintAction(action, result);
                                    //UIManager.Get.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "GetWeapon":
                                {
                                    MmoEngine.Get.Game.OnWeaponReceived(result);
                                    //UIManager.Get.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "GetInventory":
                                {
                                    //parse inventory 
                                    ClientInventory inventory = new ClientInventory(result);
                                    Dbg.Print("BEFORE REPLACING INVENTORY");
                                    Dbg.Print(inventory.ToString());
                                    //replace player inventory with new inventory
                                    MmoEngine.Get.Game.Inventory.Replace(inventory);
                                    Dbg.Print("AFTER REPLACING INVEMTRORY");
                                    Dbg.Print(MmoEngine.Get.Game.Inventory.ToString());
                                    //UIManager.Get.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "GetPlayerInfo":
                                {
                                    //received player info on request NetworkGame.RequestPlayerInfo(), parse info from server here
                                    MmoEngine.Get.Game.PlayerInfo.ParseInfo(result);
                                }
                                break;
                            case "GetSkillBinding":
                                {
                                    Debug.Log("GetSkillBinding called".Color(Color.yellow));
                                    game.Skills.ParseInfo(result);
                                    //game.Ship.Skills.ParseInfo(result);
                                    //UIManager.Get.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "DestroyModule":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "CmdAddOres":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "AddInventorySlots":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "DestroyInventoryItem":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "Rebuild":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "EquipWeaponFromInventory":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "ClearStationHold":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "ClearInventory":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "RebuildForDemo":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "AddDemoSchemes":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "AddDemoModules":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "DemoPrepare":
                                {
                                    //G.UI.ActionResult.SetActionResult(result);
                                }
                                break;
                            case "GenWeapon":
                                {
                                    //print response from GenWeapon RPC method
                                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                                    CommonUtils.ConstructHashString(result, 1, ref stringBuilder);
                                    Debug.Log("GenWeapon():\n" + stringBuilder.ToString());
                                }
                                break;
                            case "GetCombatParams":
                                {
                                    G.Game.CombatStats.ParseInfo(result);
                                }
                                break;
                        }
                    }
                    break;
            }
            //throw new System.NotImplementedException();
            */

            #endregion
        }

        public void OnPeerStatusCallback(NetworkGame game, StatusCode returnCode) {
            switch (returnCode) {
                case ExitGames.Client.Photon.StatusCode.Disconnect:
                case ExitGames.Client.Photon.StatusCode.DisconnectByServer:
                case ExitGames.Client.Photon.StatusCode.DisconnectByServerLogic:
                case ExitGames.Client.Photon.StatusCode.DisconnectByServerUserLimit:
                    {
                        game.SetDisconnected(returnCode);
                        break;
                    }
                default:
                    {
                        game.DebugReturn(DebugLevel.ERROR, returnCode.ToString());
                        break;
                    }
            }
        }

        public void OnUpdate(NetworkGame gameLogic) {
            gameLogic.Peer.Service();
        }

        public void SendOperation(NetworkGame game, OperationCode operationCode, System.Collections.Generic.Dictionary<byte, object> parameter, bool sendReliable, byte channelId) {
            game.Peer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }

    }
}


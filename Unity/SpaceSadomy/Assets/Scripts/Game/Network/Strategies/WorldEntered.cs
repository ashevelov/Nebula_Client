// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldEntered.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The dispatcher world entered.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Nebula {
    using Common;
    using ExitGames.Client.Photon;
    using System;
    using System.Collections.Generic;
    //using Photon.Mmo.Common;
    using UnityEngine;


    /// <summary>
    /// The dispatcher world entered.
    /// </summary>
    [CLSCompliant(false)]
    public class WorldEntered : IGameLogicStrategy
    {
        /// <summary>
        /// The instance.
        /// </summary>
        public static readonly IGameLogicStrategy Instance = new WorldEntered();
        private StringSubCache<string> stringSubCache = new StringSubCache<string>();
        private readonly Dictionary<EventCode, IServerEventStrategy> eventStrategies;
        private readonly Dictionary<OperationCode, OperationReturnStrategy> operationStrategies;

        public WorldEntered()
        {
            this.eventStrategies = new Dictionary<EventCode, IServerEventStrategy>();
            this.eventStrategies.Add(EventCode.UseSkill, new UseSkillEventStrategy());
            this.eventStrategies.Add(EventCode.RadarUpdate, new RadarUpdateEventStrategy());
            this.eventStrategies.Add(EventCode.ItemMoved, new ItemMovedEventStrategy());
            this.eventStrategies.Add(EventCode.ItemDestroyed, new ItemDestroyedEventStrategy());
            this.eventStrategies.Add(EventCode.ItemProperties, new ItemPropertiesEventStrategy());
            this.eventStrategies.Add(EventCode.ItemPropertiesSet, new ItemPropertiesSetEventStrategy());
            this.eventStrategies.Add(EventCode.ItemSubscribed, new ItemSubscribedEventStrategy());
            this.eventStrategies.Add(EventCode.ItemUnsubscribed, new ItemUnsubscribedEventStrategy());
            this.eventStrategies.Add(EventCode.WorldExited, new WorldExitedEventStrategy());

            ItemGenericEventStrategy genericEventStrategy = new ItemGenericEventStrategy();
            genericEventStrategy.AddStrategy(CustomEventCode.Fire, new FireGenericEventStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.WorldEventStageChanged, new GenericEventWorldEventStageChangedStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.WorldEventStartedWithStage, new GenericEventWorldEventStartedWithStageStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.WorldEventCompletedWithStage, new GenericEventWorldEventCompletedWithStageStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.WorldEventMakeTransition, new GenericEventWorldEventMakeTransitionStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.ChatMessage, new GenericEventChatMessageStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.InventoryItemsAdded, new GenericEventInventoryItemsAddedStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.InventoryUpdated, new GenericEventInventoryUpdatedStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.ShipModelUpdated, new GenericEventShipModelUpdatedStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.ServiceMessage, new GenericEventServiceMessageStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.PlayerInfoUpdated, new GenericEventPlayerInfoUpdatedStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.SkillsUpdated, new GenericEventSkillsUpdatedStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.ItemShipDestroyed, new GenericEventItemShipDestroyedStrategy());
            genericEventStrategy.AddStrategy(CustomEventCode.MailBoxUpdated, new GenericEventMailBoxUpdatedStrategy());

            WorldEnteredGenericGroupStrategy groupStr = new WorldEnteredGenericGroupStrategy();

            genericEventStrategy.AddStrategy(CustomEventCode.CooperativeGroupRequest, groupStr);
            genericEventStrategy.AddStrategy(CustomEventCode.CooperativeGroupUpdate, groupStr);


            this.eventStrategies.Add(EventCode.ItemGeneric, genericEventStrategy);

            this.operationStrategies = new Dictionary<OperationCode, OperationReturnStrategy>();
            this.operationStrategies.Add(OperationCode.RemoveInterestArea, new WERemoveInterestAreaStrategy());
            this.operationStrategies.Add(OperationCode.AddInterestArea, new WEAddInterestAreaStrategy());
            this.operationStrategies.Add(OperationCode.AttachInterestArea, new WEAttachInterestAreaStrategy());
            this.operationStrategies.Add(OperationCode.DetachInterestArea, new WEDetachInterestAreaStrategy());
            this.operationStrategies.Add(OperationCode.SpawnItem, new WESpawnItemStrategy());

            WERPCActionStrategy rpcStrategy = new WERPCActionStrategy();
            rpcStrategy.AddRPCActionStrategy("GetSkillBinding", new WERPCGetSkillBindingStrategy());
            rpcStrategy.AddRPCActionStrategy("RequestWorldEvents", new WERPCRequestWorldEventsStrategy());
            rpcStrategy.AddRPCActionStrategy("RequestContainer", new WERPCRequestContainerStrategy());
            rpcStrategy.AddRPCActionStrategy("GetInventory", new WERPCGetInventoryStrategy());
            rpcStrategy.AddRPCActionStrategy("AddFromContainer", new WERPCAddFromContainerStrategy());
            rpcStrategy.AddRPCActionStrategy("RequestShipModel", new WERPCRequestShipModelStrategy());
            rpcStrategy.AddRPCActionStrategy("GetBonuses", new WERPCGetBonusesStrategy());
            rpcStrategy.AddRPCActionStrategy("GetBonusesOnTarget", new WERPCGetBonusesOnTargetStrategy());
            rpcStrategy.AddRPCActionStrategy("TestUseSkill", new WERPCTestUseSkillStrategy());
            rpcStrategy.AddRPCActionStrategy("GetWeapon", new WERPCGetWeaponStrategy());
            rpcStrategy.AddRPCActionStrategy("GetPlayerInfo", new WERPCGetPlayerInfoStrategy());
            rpcStrategy.AddRPCActionStrategy("MoveAsteroidToInventory", new WERPCMoveAsteroidToInventoryStrategy());
            rpcStrategy.AddRPCActionStrategy("CmdAddOres", new WERPCEmptyStrategy());
            
            
            rpcStrategy.AddRPCActionStrategy("Rebuild", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("EquipWeapon", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("OnActivate", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("GetChatUpdate", new WERPCGetChatUpdateStrategy());
            rpcStrategy.AddRPCActionStrategy("TestOut", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("OnShiftDown", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("ClearStationHold", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("ClearInventory", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("RebuildForDemo", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("AddDemoSchemes", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("AddDemoModules", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("DemoPrepare", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("GenWeapon", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("GetTargetCombatProperties", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("TestApplyTimedBuffToTarget", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("TestGetTimedBuffOnTarget", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("AddHealth", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("MoveAsteroidItemToInventory", new WERPCMoveAsteroidItemToInventoryStrategy());
            
            rpcStrategy.AddRPCActionStrategy("SetCooldownMult", new WERPCEmptyStrategy());
            rpcStrategy.AddRPCActionStrategy("Respawn", new WERPCRespawnStrategy());
            rpcStrategy.AddRPCActionStrategy("GetCombatParams", new WERPCGetCombatStatsStrategy());
            rpcStrategy.AddRPCActionStrategy("AddAllFromContainer", new WERPCAddAllFromContainerStrategy());

            WERPCGroupMethodsStrategy groupMethodsStrategy = new WERPCGroupMethodsStrategy();
            rpcStrategy.AddRPCActionStrategy("TakeMailAttachment", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("RemoveMailMessage", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("SetTarget", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("TestBuffs", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("ToggleGodMode", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("AddScheme", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("DestroyInventoryItem", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("TransformObjectAndMoveToHold", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("AddInventorySlots", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("SendInviteToGroup", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("ResponseToGroupRequest", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("ExitFromCurrentGroup", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("FreeGroup", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("SetLeaderToCharacter", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("VoteForExclude", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("SetGroupOpened", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("RequestOpenedGroups", groupMethodsStrategy);
            rpcStrategy.AddRPCActionStrategy("JoinToOpenedGroup", groupMethodsStrategy);


            this.operationStrategies.Add(OperationCode.ExecAction, rpcStrategy);
            this.operationStrategies.Add(OperationCode.SetViewDistance, new WESetViewDistanceStrategy());
            this.operationStrategies.Add(OperationCode.RaiseGenericEvent, new WERaiseGenericEventStrategy());

        }

        /// <summary>
        /// Gets State.
        /// </summary>
        public GameState State
        {
            get
            {
                return GameState.WorldEntered;
            }
        }

        #region Implemented Interfaces

        #region IGameLogicStrategy

        /// <summary>
        /// The on event receive.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void OnEventReceive(NetworkGame game, EventData eventData)
        {
            IServerEventStrategy strategy = null;
            if(this.eventStrategies.TryGetValue((EventCode)eventData.Code, out strategy))
            {
                strategy.Handle(game, eventData);
            }
            else
            {
                Debug.LogWarning("Unhandled event in WorldEntered: {0}".f((EventCode)eventData.Code));
            }
            #region REFACTORED CODE
            /*
            switch ((Common.EventCode)eventData.Code)
            {
                case EventCode.RadarUpdate:
                    {
                        HandleEventRadarUpdate(eventData.Parameters, game);
                        return;
                    }

                case EventCode.ItemMoved:
                    {
                        HandleEventItemMoved(game, eventData.Parameters);
                        return;
                    }

                case EventCode.ItemDestroyed:
                    {
                        HandleEventItemDestroyed(game, eventData.Parameters);
                        return;
                    }

                case EventCode.ItemProperties:
                    {
                        HandleEventItemProperties(game, eventData.Parameters);
                        return;
                    }

                case EventCode.ItemPropertiesSet:
                    {
                        HandleEventItemPropertiesSet(game, eventData.Parameters);
                        return;
                    }

                case EventCode.ItemSubscribed:
                    {
                        HandleEventItemSubscribed(game, eventData.Parameters);
                        return;
                    }

                case EventCode.ItemUnsubscribed:
                    {
                        HandleEventItemUnsubscribed(game, eventData.Parameters);
                        return;
                    }

                case EventCode.WorldExited:
                    {
                        if (game.WorldTransition.HasNextWorld())
                        {
                            game.SetWaitChangingWorld();
                        }
                        else
                        {
                            game.SetConnected();
                        }
                        return;
                    }
                case EventCode.UseSkill:
                    {
                        try
                        {
                            Hashtable skillProperties = eventData.Parameters[(byte)ParameterCode.Properties] as Hashtable;

                            bool success = skillProperties.GetValue<bool>(GenericEventProps.success, false);
                            Hashtable castInfp = skillProperties.GetValue<Hashtable>(GenericEventProps.cast_info, new Hashtable());

                            if (success)
                            {
                                string source = skillProperties.GetValue<string>(GenericEventProps.source, string.Empty);
                                byte sourceType = skillProperties.GetValue<byte>(GenericEventProps.source_type, (byte)0);

                                Hashtable skillData = skillProperties.GetValue<Hashtable>(GenericEventProps.data, new Hashtable());
                                string skillId = skillData.GetValue<string>(GenericEventProps.id, string.Empty);

                                Item sourceItem;
                                if (G.Game.TryGetItem(sourceType, source, out sourceItem))
                                {
                                    sourceItem.UseSkill(skillProperties);
                                }
                                Dbg.Print(string.Format("Use skill event: {0}", skillId));
                            }
                            else
                            {
                                //Debug.LogError("skill failed with msg: {0}".f(skillProperties.GetValue<string>(GenericEventProps.message, string.Empty)));
                            }
                            //UIManager.Get.ActionResult.SetActionResult(castInfp);
                            StringBuilder sb = new StringBuilder();
                            CommonUtils.ConstructHashString(castInfp, 1, ref sb);
                            Dbg.Print(sb.ToString());
                        }
                        catch (InvalidCastException exception) 
                        {
                            Debug.Log("invalid cast");
                        }

                    }
                    break;
                case EventCode.ItemGeneric:
                    {
                        //Debug.Log(string.Format("EVENT GENERIC RECEIVED {0}", (CustomEventCode)eventData.Parameters[(byte)ParameterCode.CustomEventCode]));
                        CustomEventCode customCode = (CustomEventCode)eventData.Parameters[(byte)ParameterCode.CustomEventCode];
                        switch (customCode) 
                        { 
                            case CustomEventCode.Fire:
                                {
                                    Hashtable properties = eventData.Parameters[(byte)ParameterCode.EventData] as Hashtable;
                                    if (properties != null)
                                    {
                                        bool fireBlocked = properties.GetValue<bool>(GenericEventProps.fire_blocked, false);
                                        bool fireAllowed = properties.GetValue<bool>(GenericEventProps.fire_allowed, true);

                                        if (fireAllowed)
                                        {
                                            if (false == fireBlocked)
                                            {
                                                properties.Print(1);
                                                game.OnItemFire(properties);
                                            }
                                            else
                                            {
                                                //UIManager.Get.ActionResult.SetActionResult(new Hashtable { { "fire result: ", "fire blocked" } });
                                            }
                                        }
                                        else
                                        {
                                            //G.UI.ActionResult.SetActionResult(new Hashtable { { "ERR", "Shoot not allow on this player" } });
                                        }
                                    }
                                    else {
                                        //Debug.Log("PROPETIES NULL in FIRE EVENT");
                                    }
                                }
                                break;
                            case CustomEventCode.WorldEventStageChanged:
                                {
                                    Hashtable eventInfo = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;

                                    game.WorldEventConnection.SetEvent(eventInfo);
                                    string id = eventInfo.GetValue<string>(GenericEventProps.id, string.Empty);
                                    string worldId = eventInfo.GetValue<string>(GenericEventProps.WorldId, string.Empty);

                                    if(!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(worldId))
                                    {
                                        ClientWorldEventInfo worldEventInfo = game.WorldEventConnection.GetEvent(worldId, id);
                                        if (worldEventInfo != null)
                                        {
                                            if (worldEventInfo.Stage != null)
                                            {
                                                string startTextId = worldEventInfo.Stage.StartTextId;
                                                if (!string.IsNullOrEmpty(startTextId))
                                                {
                                                    string startText = stringSubCache.String(startTextId, startTextId);
                                                    startText = startText.ReplaceVariables(worldEventInfo.VariablesInfo);
                                                    EventTextView textView = new EventTextView(G.UI, startText);

                                                    GameObject.Instantiate(PrefabCache.Get("Prefabs/Event_Mark"), 
                                                        new Vector3(worldEventInfo.Position[0], worldEventInfo.Position[1], worldEventInfo.Position[2]), 
                                                        Quaternion.identity);
                                                }
                                                else
                                                    Debug.LogError("start text id is null or empty");
                                            }
                                            else
                                                Debug.LogError("event info stage null");
                                        }
                                        else
                                            Debug.LogError("world event info not founded");
                                    }
                                }
                                break;
                            case CustomEventCode.WorldEventStartedWithStage:
                                {
                                    Hashtable info = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    game.WorldEventConnection.SetEvent(info);
                                }
                                break;
                            case CustomEventCode.WorldEventCompletedWithStage:
                                {
                                    Hashtable info = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    game.WorldEventConnection.SetEvent(info);
                                }
                                break;
                            case CustomEventCode.WorldEventMakeTransition:
                                {
                                    Hashtable info = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    string transitionText = info.GetValue<string>("transition_text", string.Empty);
                                    if(!string.IsNullOrEmpty(transitionText))
                                    {
                                        new EventTextView(G.UI, stringSubCache.String(transitionText, transitionText));
                                    }
                                }
                                break;
                            case CustomEventCode.ChatMessage:
                                {
                                    //Debug.Log("chat message received");
                                    //game.Chat.ReceiveMessage(eventData.Parameters[(byte)ParameterCode.EventData] as Hashtable);
                                }
                                break;
                            case CustomEventCode.InventoryItemsAdded:
                                {
                                    //sended to player when action  "AddFromContainer" completed and return items 
                                    Hashtable result = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    string containerItemId = result.GetValue<string>(GenericEventProps.target_id, string.Empty);
                                    byte containerItemType = result.GetValue<byte>(GenericEventProps.target_type, 0);
                                    object[] addedObjects = result.GetValue<object[]>(GenericEventProps.InventoryItems, new object[] { });

                                    //parse added inventory items
                                    List<ClientInventoryItem> inventoryItems = InventoryObjectInfoFactory.ParseItemsArray(addedObjects);
                                    //add to inventroy and remove from container
                                    game.AddToInventoryTransaction(containerItemId, containerItemType, inventoryItems);
                                    //for debug print current inventory
                                    Debug.Log(game.Inventory.ToString());
                                }
                                break;
                            case CustomEventCode.InventoryUpdated:
                                {
                                    Hashtable result = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    Dbg.PrintGenericEvent(CustomEventCode.InventoryUpdated, result);
                                    NetworkGame.OnInventoryUpdated(game, result);
                                }
                                break;
                            case CustomEventCode.ShipModelUpdated:
                                {
                                    Hashtable result = eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable;
                                    NetworkGame.OnShipModelUpdated(game, result);
                                }
                                break;
                            case CustomEventCode.ServiceMessage:
                                {
                                    //Debug.Log("service message received".Color("green"));
                                    game.ServiceMessageReceiver.AddMessage(eventData.Parameters[ParameterCode.EventData.toByte()] as Hashtable);
                                    G.UI.ChatView.SetNeedScrollLogToLastMessage(true);
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
                                    StringBuilder sb = new StringBuilder();
                                    CommonUtils.ConstructHashString(info, 1, ref sb);
                                    Debug.Log(sb.ToString().Color(Color.green));
                                }
                                break;
                            case CustomEventCode.ItemShipDestroyed:
                                {
                                    Debug.Log("<color=orange>Item ship destroyed event received</color>");
                                    bool shipDestroyed = (bool)eventData.Parameters[ParameterCode.EventData.toByte()];
                                    string itemId = (string)eventData.Parameters[ParameterCode.ItemId.toByte()];
                                    byte itemType = (byte)eventData.Parameters[ParameterCode.ItemType.toByte()];
                                    Item targetItem;
                                    if(G.Game.TryGetItem(itemType, itemId, out targetItem))
                                    {
                                        targetItem.SetShipDestroyed(shipDestroyed);
                                    }
                                }
                                break;
                        }

                    }
                    break;
                default:
                    {
                        Debug.Log(string.Format("Unexpected event: {0}", (EventCode)eventData.Code));
                    }
                    break;
            }
            */
            #endregion
            //game.OnUnexpectedEventReceive(eventData);
        }




        /// <summary>
        /// The on operation return.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="response">
        /// The operation response.
        /// </param>
        public void OnOperationReturn(NetworkGame game, OperationResponse response)
        {
            if (response.ReturnCode == 0)
            {
                OperationCode code = (OperationCode)response.OperationCode;
                if (this.operationStrategies.ContainsKey(code))
                    this.operationStrategies[code].Handle(game, response);

                /*
                #region OLD SWITCH
                switch ((OperationCode)response.OperationCode)
                {
                    case OperationCode.RemoveInterestArea:
                    case OperationCode.AddInterestArea:
                        {
                            return;
                        }

                    case OperationCode.AttachInterestArea:
                        {
                            HandleEventInterestAreaAttached(game, response.Parameters);
                            return;
                        }

                    case OperationCode.DetachInterestArea:
                        {
                            HandleEventInterestAreaDetached(game);
                            return;
                        }

                    case OperationCode.SpawnItem:
                        {
                            HandleEventItemSpawned(game, response.Parameters);
                            return;
                        }

                    case OperationCode.RadarSubscribe:
                        {
                            return;
                        }
                    case OperationCode.ExecAction:
                        {


                            string action = (string)response[(byte)ParameterCode.Action];
                            Hashtable result = (Hashtable)response[(byte)ParameterCode.Result];
                            switch (action) {
                                case "GetSkillBinding":
                                    {
                                        game.Skills.ParseInfo(result);
                                    }
                                    break;
                                case "RequestWorldEvents":
                                    {
                                        Dbg.Print("receive requested events");
                                        if (result != null)
                                        {
                                            result.Print(1);
                                            game.WorldEventConnection.ParseInfo(result);
                                        }
                                    }
                                    break;
                                case "RequestContainer":
                                    {
                                        Dbg.Print("RequestContainer() response received");
                                        List<Game.Space.IInventoryObjectInfo> containerList = new List<Game.Space.IInventoryObjectInfo>();
                                        string containerId = string.Empty;
                                        byte containerType = (byte)ItemType.Avatar;

                                        List<ClientInventoryItem> inventoryItems = new List<ClientInventoryItem>();

                                        if (result != null) 
                                        {
                                            foreach (DictionaryEntry entry in result) 
                                            {

                                                if (entry.Value is Hashtable) 
                                                {
                                                    
                                                    Game.Space.IInventoryObjectInfo objInfo = Game.Space.InventoryObjectInfoFactory.Get(entry.Value as Hashtable);
                                                    if (objInfo != null)
                                                    {
                                                        Debug.Log("<color=orange>Object Info in container created</color>");
                                                        containerList.Add(objInfo);

                                                        inventoryItems.Add(new ClientInventoryItem(objInfo, 1));
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("<color=orange>Object Info is null</color>");
                                                    }
                                                }
                                                else if (entry.Key.ToString() == GenericEventProps.target_id) {
                                                    containerId = (string)entry.Value;
                                                }
                                                else if (entry.Key.ToString() == GenericEventProps.target_type) {
                                                    containerType = (byte)entry.Value;
                                                }
                                            }
                                        }

                                        if(string.IsNullOrEmpty(containerId))
                                        {
                                            //Debug.Log("RequestContainer: container id is empty");
                                            return;
                                        }
                                        Item item;
                                        if( G.Game.TryGetItem(containerType, containerId, out item) )
                                        {
                                            if(item is IInventoryItemsSource)
                                            {
                                                (item as IInventoryItemsSource).SetItems(inventoryItems);
                                                G.Game.CurrentObjectContainer.Reset();
                                                G.Game.CurrentObjectContainer.SetContainer(containerId, containerType, containerList);

                                            }
                                        }


                                    }
                                    break;
                                case "GetInventory":
                                    {
                                        //Debug.Log(result.ToStringBuilder().ToString());
                                        //parse inventory 
                                        ClientInventory inventory = new ClientInventory(result);
                                        //Debug.Log("BEFORE REPLACING INVENTORY");
                                        //Debug.Log(inventory.ToString());
                                        //replace player inventory with new inventory
                                        MmoEngine.Get.Game.Inventory.Replace(inventory);
                                        //Debug.Log("AFTER REPLACING INVEMTRORY");
                                        //Debug.Log(MmoEngine.Get.Game.Inventory);
                                    }
                                    break;
                                case "AddFromContainer":
                                    {
                                        string status = result.GetValue<string>("status", "unknown");
                                        Debug.Log(string.Format("<color=orange>AddFromContainer() status: {0}</color>", status));
                                    }
                                    break;
                                case "RequestShipModel":
                                    {
                                        NetworkGame.OnShipModelUpdated(game, result);
                                    }
                                    break;
                                case "GetBonuses":
                                    {

                                        MmoEngine.Get.Game.Bonuses.Replace(result);
                                    }
                                    break;
                                case "GetBonusesOnTarget":
                                    {
                                        Debug.Log("GetBonusesOnTarget()".Color("orange").Bold());
                                        foreach (DictionaryEntry entry in result) {
                                            Debug.Log(string.Format("{0}:{1}", (Common.BonusType)entry.Key, entry.Value));
                                        }
                                    }
                                    break;
                                case "TestUseSkill":
                                    {
                                        Debug.Log("TestUseSkill()".Color("orange").Bold());
                                        foreach (DictionaryEntry entry in result) {
                                            Debug.Log(string.Format("{0}:{1}", entry.Key, entry.Value));
                                        }
                                        //UIManager.Get.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "GetWeapon":
                                    {
                                        MmoEngine.Get.Game.OnWeaponReceived(result);
                                    }
                                    break;
                                case "GetPlayerInfo":
                                    {
                                        //received player info on request NetworkGame.RequestPlayerInfo(), parse info from server here
                                        MmoEngine.Get.Game.PlayerInfo.ParseInfo(result);
                                    }
                                    break;
                                case "MoveAsteroidToInventory":
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        CommonUtils.ConstructHashString(result, 1, ref sb);
                                        Debug.Log("MoveAsteroidToInventory");
                                        Debug.Log(sb.ToString());
                                        //UIManager.Get.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "CmdAddOres":
                                    {
                                        //UIManager.Get.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "AddInventorySlots":
                                    {
                                        //UIManager.Get.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "DestroyInventoryItem":
                                    {
                                        //UIManager.Get.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "Rebuild":
                                    {
                                        //UIManager.Get.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "EquipWeaponFromInventory":
                                    {
                                        //G.UI.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "OnActivate":
                                    {
                                        //UIManager.Get.ActionResult.SetActionResult(result);
                                    }
                                    break;
                                case "GetChatUpdate":
                                    {
                                        Dbg.Print("chat update received: {0}".f(result.Count));
                                        result.Print(1);
                                        G.Game.Chat.ReceiveUpdate(result);
                                    }
                                    break;
                                case "TestOut":
                                    {
                                        Debug.Log("TestOut() completed");
                                    }
                                    break;
                                case "OnShiftDown":
                                    {
                                        //G.UI.ActionResult.SetActionResult(new Hashtable { { "OnShiftDown()", "shift pressed"} });
                                    }
                                    break;
                                case "OnShiftUp":
                                    {
                                        //G.UI.ActionResult.SetActionResult(new Hashtable {  { "OnShiftUp()", "shift unpressed" } });
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
                                        System.Text.StringBuilder stringBuilder = new StringBuilder();
                                        CommonUtils.ConstructHashString(result, 1, ref stringBuilder);
                                        Debug.Log("GenWeapon():\n" + stringBuilder.ToString());
                                    }
                                    break;
                                case "GetTargetCombatProperties":
                                    {
                                        Debug.Log("RPC RESPONSE: GetTargetCombatProperties");
                                        System.Text.StringBuilder stringBuilder = new StringBuilder();
                                        CommonUtils.ConstructHashString(result, 1, ref stringBuilder);
                                        Debug.Log(stringBuilder.ToString());
                                    }
                                    break;
                                case "TestApplyTimedBuffToTarget":
                                    {
                                        Debug.Log("RPC RESPONSE: TestApplyTimedBuffToTarget");
                                        System.Text.StringBuilder stringBuilder = new StringBuilder();
                                        CommonUtils.ConstructHashString(result, 1, ref stringBuilder);
                                        Dbg.Print(stringBuilder.ToString(), "RPC");
                                    }
                                    break;
                                case "TestGetTimedBuffOnTarget":
                                    {
                                        Debug.Log("RPC RESPONSE: TestGetTimedBuffOnTarget");
                                        System.Text.StringBuilder stringBuilder = new StringBuilder();
                                        CommonUtils.ConstructHashString(result, 1, ref stringBuilder);
                                        Dbg.Print(stringBuilder.ToString(), "RPC");
                                    }
                                    break;
                                case "Respawn":
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        CommonUtils.ConstructHashString(result, 1, ref sb);
                                        Dbg.Print(sb.ToString(), "PLAYER");
                                        //G.Game.Avatar.Respawn();
                                        G.Game.Ship.SetNeedRespawnFlag();
                                    }
                                    break;
                                case "AddHealth":
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        CommonUtils.ConstructHashString(result, 1, ref sb);
                                        Dbg.Print(sb.ToString(), "PLAYER");
                                    }
                                    break;
                                case "GetCombatParams":
                                    {
                                        G.Game.CombatStats.ParseInfo(result);
                                    }
                                    break;
                                case "MoveAsteroidItemToInventory":
                                    {
                                        Dbg.Print((string)result[ACTION_RESULT.MESSAGE], "RPC");
                                    }
                                    break;
                                case "AddAllFromContainer":
                                    G.UI.Note.SetActionResult(new List<string> { result[ACTION_RESULT.MESSAGE].ToString() });
                                    break;
                                case "AddExp":
                                    {
                                        int oldExp = result.GetValue<int>("old", 0);
                                        int newExp = result.GetValue<int>("new", 0);
                                        Debug.Log("AddExp() completed. Exp changed from {0} to {1}".f(oldExp, newExp));
                                    }
                                    break;
                                case "ToggleGodMode":
                                    {
                                        G.UI.Note.SetActionResult(new List<string> { result[ACTION_RESULT.MESSAGE].ToString() });
                                    }
                                    break;
                                case "SetCooldownMult":
                                    {
                                        G.UI.Note.SetActionResult(new List<string> { result[ACTION_RESULT.MESSAGE].ToString() });
                                    }
                                    break;
                            }
                        }
                        break;
                    case OperationCode.SetViewDistance:
                        //MmoEngine.Get.Game.Avatar.i
                        break;
                    case OperationCode.RaiseGenericEvent:
                        break;
                    default:
                        Debug.Log(string.Format("Unexpected operation occured: {0}", (OperationCode)response.OperationCode));
                        break;
                }
                #endregion
                 * */
            }

            //game.OnUnexpectedOperationError(response);
        }

        /// <summary>
        /// The on peer status callback.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="returnCode">
        /// The return code.
        /// </param>
        public void OnPeerStatusCallback(NetworkGame game, StatusCode returnCode)
        {
            switch (returnCode)
            {
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
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

        /// <summary>
        /// The on update.
        /// </summary>
        /// <param name="game">
        /// The game logic.
        /// </param>
        public void OnUpdate(NetworkGame game)
        {
            game.Peer.Service();
        }

        /// <summary>
        /// The send operation.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="operationCode">
        /// The operation code.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="sendReliable">
        /// The send reliable.
        /// </param>
        /// <param name="channelId">
        /// The channel Id.
        /// </param>
        public void SendOperation(NetworkGame game, OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId)
        {
            game.Peer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }

        #endregion

        #endregion

        /*
        /// <summary>
        /// The handle event interest area attached.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventInterestAreaAttached(NetworkGame game, IDictionary eventData)
        {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            game.OnCameraAttached(itemId, itemType);
        }*/

        /*
        /// <summary>
        /// The handle event interest area detached.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        private static void HandleEventInterestAreaDetached(NetworkGame game)
        {
            game.OnCameraDetached();
        }*/

        /*
        /// <summary>
        /// The handle event item destroyed.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventItemDestroyed(NetworkGame game, IDictionary eventData)
        {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            //Debug.Log("item {0} of type {1} destoyed".f(itemId, itemType.toItemType()));

            Item item;
            if (game.TryGetItem(itemType, itemId, out item))
            {
                Dbg.Print(string.Format("<color=orange>item {0} of type: {1} try destroyed</color>", itemId.Substring(0, 3), (ItemType)itemType));
                item.IsDestroyed = game.RemoveItem(item);
                Dbg.Print(string.Format("<color=orange>destroyed: {0}</color>", item.IsDestroyed));
            }
        }
        */

        /*
        /// <summary>
        /// The handle event item moved.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventItemMoved(NetworkGame game, IDictionary eventData)
        {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            Item item;
            if (game.TryGetItem(itemType, itemId, out item))
            {
                if (item.IsMine == false)
                {
                    var position = (float[])eventData[(byte)ParameterCode.Position];
                    var oldPosition = (float[])eventData[(byte)ParameterCode.OldPosition];
                    float[] rotation = eventData.Contains((byte)ParameterCode.Rotation) ? (float[])eventData[(byte)ParameterCode.Rotation] : null;
                    float[] oldRotation = eventData.Contains((byte)ParameterCode.OldRotation) ? (float[])eventData[(byte)ParameterCode.OldRotation] : null;
                    float speed = eventData.Contains(ParameterCode.Speed.toByte()) ? (float)eventData[ParameterCode.Speed.toByte()] : 0f;

                    item.SetPositions(position, oldPosition, rotation, oldRotation, speed);
                    //Debug.Log("foreign item moved received");
                }
            }
        }
        */

        /*
        /// <summary>
        /// The handle event item properties.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventItemProperties(NetworkGame game, IDictionary eventData)
        {
            //game.Listener.LogInfo(game, "HandleEventItemProperties");
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            Item item;
            if (game.TryGetItem(itemType, itemId, out item))
            {
                item.PropertyRevision = (int)eventData[(byte)ParameterCode.PropertiesRevision];

                Hashtable propertiesSet = eventData[(byte)ParameterCode.PropertiesSet] as Hashtable;
                //game.Listener.LogInfo(game, "properties count: " + propertiesSet.Count);

                foreach (DictionaryEntry entry in propertiesSet)
                {
                    Hashtable groupProps = entry.Value as Hashtable;
                    if (groupProps == null)
                    {
                        game.Engine.LogError(game, string.Format("Property entry for group: {0} of item: {1} not hashtable but: {2}", entry.Key.ToString(), item.Id, entry.Value.GetType()));
                        continue;
                    }
                    item.SetProperties(entry.Key.ToString(), groupProps);
                }
                
            }
        }
        */

        /*
        /// <summary>
        /// The handle event item properties set.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventItemPropertiesSet(NetworkGame game, IDictionary eventData)
        {
            game.Engine.LogInfo(game, "HandleEventItemPropertiesSet");
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            Item item;
            if (game.TryGetItem(itemType, itemId, out item))
            {
                item.PropertyRevision = (int)eventData[(byte)ParameterCode.PropertiesRevision];

                Hashtable propertiesSet = eventData[(byte)ParameterCode.PropertiesSet] as Hashtable;
                foreach (DictionaryEntry entry in propertiesSet)
                {
                    Hashtable groupProps = entry.Value as Hashtable;
                    if (groupProps == null)
                    {
                        game.Engine.LogError(game, string.Format("Property entry for group: {0} of item: {1} not hashtable but: {2}", entry.Key.ToString(), item.Id, entry.Value.GetType()));
                        continue;
                    }
                    item.SetProperties(entry.Key.ToString(), groupProps);
                }
            }
        }
        */

        /*
        /// <summary>
        /// The handle event item spawned.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventItemSpawned(NetworkGame game, IDictionary eventData)
        {

            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            game.OnItemSpawned(itemType, itemId);

            Debug.Log(string.Format("item spawned: {0} of type: {1}", itemId, itemType.toItemType()));
        }
        */

        /*
        /// <summary>
        /// The handle event item subscribed.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventItemSubscribed(NetworkGame game, IDictionary eventData)
        {

            var itemType = eventData.GetValue((byte)ParameterCode.ItemType, (byte)ItemType.Avatar); //(byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            var position = (float[])eventData[(byte)ParameterCode.Position];
            var cameraId = (byte)eventData[(byte)ParameterCode.InterestAreaId];
            float[] rotation = eventData.Contains((byte)ParameterCode.Rotation) ? (float[])eventData[(byte)ParameterCode.Rotation] : null;
            byte subType = eventData.Contains((byte)ParameterCode.SubType) ? (byte)eventData[(byte)ParameterCode.SubType] : (byte)0;
            Hashtable itemProperties = eventData.Contains((byte)ParameterCode.Properties) ? (Hashtable)eventData[(byte)ParameterCode.Properties] : new Hashtable();

            string displayName = eventData.Contains((byte)ParameterCode.Username) ? (string)eventData[(byte)ParameterCode.Username] : string.Empty;


            //Debug.Log("item subscribed: {0} of type: {1}".f(itemId, itemType.toItemType()));

            if ((ItemType)itemType == ItemType.Avatar)
            {
                Debug.Log("avatar player subscrived: {0}".f(itemId).Bold().Color("orange"));
            }

            Item item;
            if (game.TryGetItem(itemType, itemId, out item))
            {
                item.SetSubscribed(true);

                if (item.IsMine)
                {
                    Debug.Log("MY ITEM SUBSCRIBED");
                    item.AddSubscribedInterestArea(cameraId);
                    item.AddVisibleInterestArea(cameraId);
                }
                else
                {
                    var revision = (int)eventData[(byte)ParameterCode.PropertiesRevision];
                    if (revision == item.PropertyRevision)
                    {
                        item.AddSubscribedInterestArea(cameraId);
                        item.AddVisibleInterestArea(cameraId);
                    }
                    else
                    {
                        item.AddSubscribedInterestArea(cameraId);
                        item.GetProperties(null);
                    }

                    item.SetPositions(position, position, rotation, rotation, 0);

                }
                if (!item.ExistsView) 
                {
                    MmoEngine.Get.OnItemAdded(game, item);
                }
            }
            else
            {
                switch((ItemType)itemType)
                {
                    case ItemType.Avatar:
                        item = new ForeignPlayerItem(itemId, itemType, game, displayName);
                        item.SetPositions(position, position, rotation, rotation, 0);
                        game.AddItem(item);
                        item.AddSubscribedInterestArea(cameraId);
                        item.GetProperties(null);
                        item.SetSubscribed(true);
                        break;
                    case ItemType.Bot:
                        
                        switch ((BotItemSubType)subType)
                        {
                            case BotItemSubType.StandardCombatNpc:
                                {
                                    item = new StandardNpcCombatItem(itemId, itemType, game, BotItemSubType.StandardCombatNpc, displayName);
                                    item.SetPositions(position, position, rotation, rotation, 0);
                                    item.SetSubscribed(true);
                                    foreach (DictionaryEntry entry in itemProperties)
                                    {
                                        string group = entry.Key.ToString();
                                        Hashtable groupProperties = entry.Value as Hashtable;
                                        if (groupProperties != null)
                                        {
                                            item.SetProperties(group, groupProperties);
                                        }
                                    }
                                    game.AddItem(item);
                                    item.AddSubscribedInterestArea(cameraId);
                                    item.GetProperties(null);
                                }
                                break;
                            case BotItemSubType.PirateStation:
                                {
                                    item = new PirateStationItem(itemId, itemType, game, BotItemSubType.PirateStation, displayName);
                                    item.SetPositions(position, position, rotation, rotation, 0);
                                    item.SetSubscribed(true);
                                    foreach(DictionaryEntry entry in itemProperties)
                                    {
                                        item.SetProperties(entry.Key.ToString(), entry.Value as Hashtable);
                                    }
                                    game.AddItem(item);
                                    item.AddSubscribedInterestArea(cameraId);
                                    item.GetProperties(null);
                                }
                                break;
                            case BotItemSubType.Activator:
                                {
                                    CreateActivator(itemId, itemType, game, displayName, position, rotation, cameraId);
                                }
                                break;
                        }
                        break;
                    case ItemType.Chest:
                        {
                            CreateChest(itemId, itemType, game, displayName, position, rotation, cameraId);
                        }
                        break;
                    case ItemType.Asteroid:
                        {
                            CreateAsteroid(itemId, itemType, game, displayName, position, rotation, cameraId);
                        }
                        break;
                        
                }
                
                
            }
        }

        private static void CreateChest(string id, byte type, NetworkGame game, string name, 
            float[] position, float[] rotation, byte areaId ) 
        {
            Item item = new ChestItem(id, type, game, name);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties(null);
        }

        private static void CreateActivator(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId)
        {
            //Debug.Log("Activator subscribed");
            var item = new WorldActivatorItem(id, type, game, name);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties(null);
        }

        private static void CreateAsteroid(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId)
        {
            Item item = new AsteroidItem(id, type, game, name);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties(new string[] { GroupProps.ASTEROID });
        }

        /// <summary>
        /// The handle event item unsubscribed.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private static void HandleEventItemUnsubscribed(NetworkGame game, IDictionary eventData)
        {

            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            var cameraId = (byte)eventData[(byte)ParameterCode.InterestAreaId];

            //Debug.Log("item {0} of type {1} unsibscribed".f(itemId, itemType.toItemType()));

            Item item;
            if (game.TryGetItem(itemType, itemId, out item))
            {
                //if (false == item.IsMine)
                //{
                    if (item.RemoveSubscribedInterestArea(cameraId))
                    {
                        item.RemoveVisibleInterestArea(cameraId);
                    }

                    //Debug.Log("<color=green>ITEM UNSUBSCRIBED</color>");
                    if (item.ExistsView)
                    {
                        item.Component.ReleaseGUI();
                        item.DestroyView();
                    }
                    item.SetSubscribed(false);
                //}
            }

            
        }

        /// <summary>
        /// The handle event radar update.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        private static void HandleEventRadarUpdate(IDictionary eventData, NetworkGame game)
        {
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var position = (float[])eventData[(byte)ParameterCode.Position];
            game.Engine.OnRadarUpdate(itemId, itemType, position);
        }*/
    }
}
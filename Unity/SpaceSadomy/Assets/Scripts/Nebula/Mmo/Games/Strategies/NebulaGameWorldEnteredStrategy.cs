// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldEntered.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The dispatcher world entered.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Nebula.Mmo.Games.Strategies {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using Nebula.Mmo.Games.Strategies.Events;
    using Nebula.Mmo.Games.Strategies.Events.Game;
    using Nebula.Mmo.Games.Strategies.Operations;
    using Nebula.Mmo.Games.Strategies.Operations.Game;
    using Nebula.Resources;

    public class NebulaGameWorldEnteredStrategy : DefaultStrategy
    {
        private StringSubCache<string> stringSubCache = new StringSubCache<string>();

        private ItemMovedEvent itemMovedEvent;


        public NebulaGameWorldEnteredStrategy()
        {
            AddEventHandler((byte)EventCode.UseSkill, new UseSkillEvent());
            itemMovedEvent = new ItemMovedEvent();
            AddEventHandler((byte)EventCode.ItemMoved, itemMovedEvent);
            AddEventHandler((byte)EventCode.ItemDestroyed, new ItemDestroyedEvent());
            AddEventHandler((byte)EventCode.ItemProperties, new ItemPropertiesEvent());
            AddEventHandler((byte)EventCode.ItemPropertiesSet, new ItemPropertiesSetEvent());
            AddEventHandler((byte)EventCode.ItemSubscribed, new ItemSubscribedEvent());
            AddEventHandler((byte)EventCode.ItemUnsubscribed, new ItemUnsubscribedEvent());
            AddEventHandler((byte)EventCode.WorldExited, new WorldExitedEvent());

            GenericContainerEvent generic = new GenericContainerEvent();
            generic.AddStrategy(CustomEventCode.Fire, new FireEvent());
            //generic.AddStrategy(CustomEventCode.WorldEventStageChanged, new WorldEventStageChangedEvent());
            //generic.AddStrategy(CustomEventCode.WorldEventStartedWithStage, new WorldEventStartedWithStageEvent());
            //generic.AddStrategy(CustomEventCode.WorldEventCompletedWithStage, new WorldEventCompletedWithStageEvent());
            //generic.AddStrategy(CustomEventCode.WorldEventMakeTransition, new WorldEventMakeTransitionEvent());
            generic.AddStrategy(CustomEventCode.GameEvent, new GameEventStatusEvent());
            generic.AddStrategy(CustomEventCode.ChatMessage, new ChatMessageEvent());
            generic.AddStrategy(CustomEventCode.InventoryItemsAdded, new InventoryItemsAddedEvent());
            generic.AddStrategy(CustomEventCode.InventoryUpdated, new InventoryUpdatedEvent());
            generic.AddStrategy(CustomEventCode.ShipModelUpdated, new ShipModelUpdatedEvent());
            generic.AddStrategy(CustomEventCode.ServiceMessage, new ServiceMessageEvent());
            generic.AddStrategy(CustomEventCode.PlayerInfoUpdated, new PlayerInfoUpdatedEvent());
            generic.AddStrategy(CustomEventCode.SkillsUpdated, new SkillsUpdatedEvent());
            generic.AddStrategy(CustomEventCode.ItemShipDestroyed, new ItemShipDestroyedEvent());
            //generic.AddStrategy(CustomEventCode.MailBoxUpdated, new MailBoxUpdatedEvent());

            GenericMultiEvent multiEvent = new GenericMultiEvent();
            generic.AddStrategy(CustomEventCode.CooperativeGroupRequest, multiEvent);
            generic.AddStrategy(CustomEventCode.CooperativeGroupUpdate, multiEvent);
            AddEventHandler((byte)EventCode.ItemGeneric, generic);

            AddOperationHandler((byte)OperationCode.RemoveInterestArea, new RemoveInterestAreaOperation());
            AddOperationHandler((byte)OperationCode.AddInterestArea, new AddInterestAreaOperation());
            AddOperationHandler((byte)OperationCode.AttachInterestArea, new AttachInterestAreaOperation());
            AddOperationHandler((byte)OperationCode.DetachInterestArea, new DetachInterestAreaOperation());
            AddOperationHandler((byte)OperationCode.SpawnItem, new SpawnItemOperation());

            GenericContainerOperation container = new GenericContainerOperation();
            GenericMultiOperation multiOp = new GenericMultiOperation();

            container.AddRPCActionStrategy("GetSkillBinding", new GetSkillBindingOperation());
            //container.AddRPCActionStrategy("RequestWorldEvents", new RequestWorldEventsOperation());
            container.AddRPCActionStrategy("RequestContainer", new RequestContainerOperation());
            container.AddRPCActionStrategy("GetInventory", new GetInventoryOperation());
            container.AddRPCActionStrategy("AddFromContainer", new AddFromContainerOperation());
            container.AddRPCActionStrategy("RequestShipModel", new RequestShipModelOperation());
            container.AddRPCActionStrategy("GetBonuses", new GetBonusesOperation());
            container.AddRPCActionStrategy("GetBonusesOnTarget", new GetBonusesOnTargetOperation());
            container.AddRPCActionStrategy("TestUseSkill", new TestUseSkillOperation());
            container.AddRPCActionStrategy("GetWeapon", new GetWeaponOperation());
            container.AddRPCActionStrategy("GetPlayerInfo", new GetPlayerInfoOperation());
            container.AddRPCActionStrategy("MoveAsteroidToInventory", new MoveAsteroidToInventoryOperation());
            container.AddRPCActionStrategy("CmdAddOres", new BaseGenericOperation());
            container.AddRPCActionStrategy("Rebuild", new BaseGenericOperation());
            container.AddRPCActionStrategy("EquipWeapon", new BaseGenericOperation());
            container.AddRPCActionStrategy("OnActivate", new BaseGenericOperation());
            container.AddRPCActionStrategy("GetChatUpdate", new BaseGenericOperation());
            container.AddRPCActionStrategy("TestOut", new BaseGenericOperation());
            container.AddRPCActionStrategy("OnShiftDown", new BaseGenericOperation());
            container.AddRPCActionStrategy("ClearStationHold", new BaseGenericOperation());
            container.AddRPCActionStrategy("ClearInventory", new BaseGenericOperation());
            container.AddRPCActionStrategy("RebuildForDemo", new BaseGenericOperation());
            container.AddRPCActionStrategy("AddDemoSchemes", new BaseGenericOperation());
            container.AddRPCActionStrategy("AddDemoModules", new BaseGenericOperation());
            container.AddRPCActionStrategy("DemoPrepare", new BaseGenericOperation());
            container.AddRPCActionStrategy("GenWeapon", new BaseGenericOperation());
            container.AddRPCActionStrategy("GetTargetCombatProperties", new BaseGenericOperation());
            container.AddRPCActionStrategy("TestApplyTimedBuffToTarget", new BaseGenericOperation());
            container.AddRPCActionStrategy("TestGetTimedBuffOnTarget", new BaseGenericOperation());
            container.AddRPCActionStrategy("AddHealth", new BaseGenericOperation());
            container.AddRPCActionStrategy("MoveAsteroidItemToInventory", new MoveAsteroidItemToInventoryOperation());
            container.AddRPCActionStrategy("SetCooldownMult", new BaseGenericOperation());
            container.AddRPCActionStrategy("Respawn", new RespawnOperation());
            container.AddRPCActionStrategy("GetCombatParams", new GetCombatStatsOperation());
            container.AddRPCActionStrategy("AddAllFromContainer", new AddAllFromContainerOperation());
            container.AddRPCActionStrategy("TakeMailAttachment", multiOp);
            container.AddRPCActionStrategy("RemoveMailMessage", multiOp);
            container.AddRPCActionStrategy("SetTarget", multiOp);
            container.AddRPCActionStrategy("TestBuffs", multiOp);
            container.AddRPCActionStrategy("TGM", multiOp);
            container.AddRPCActionStrategy("AddScheme", multiOp);
            container.AddRPCActionStrategy("DestroyInventoryItem", multiOp);
            container.AddRPCActionStrategy("TransformObjectAndMoveToHold", multiOp);
            container.AddRPCActionStrategy("AddInventorySlots", multiOp);
            container.AddRPCActionStrategy("SendInviteToGroup", multiOp);
            container.AddRPCActionStrategy("ResponseToGroupRequest", multiOp);
            container.AddRPCActionStrategy("ExitFromCurrentGroup", multiOp);
            container.AddRPCActionStrategy("FreeGroup", multiOp);
            container.AddRPCActionStrategy("SetLeaderToCharacter", multiOp);
            container.AddRPCActionStrategy("VoteForExclude", multiOp);
            container.AddRPCActionStrategy("SetGroupOpened", multiOp);
            container.AddRPCActionStrategy("RequestOpenedGroups", multiOp);
            container.AddRPCActionStrategy("JoinToOpenedGroup", multiOp);
            container.AddRPCActionStrategy("DestroyInventoryItems", multiOp);
            container.AddRPCActionStrategy("AddInventoryItem", multiOp);

            AddOperationHandler((byte)OperationCode.ExecAction, container);
            AddOperationHandler((byte)OperationCode.SetViewDistance, new SetViewDistanceOperation());
            AddOperationHandler((byte)OperationCode.RaiseGenericEvent, new RaiseGenericEventOperation());
        }

        public override void OnEventReceive(BaseGame game, EventData eventData) {
            if (eventData.Code == (byte)EventCode.ItemMoved) {
                itemMovedEvent.Handle(game, eventData);
            } else {
                base.OnEventReceive(game, eventData);
            }
        }

        public override GameState State {
            get {
                return GameState.NebulaGameWorldEntered;
            }
        }

        public override void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            base.OnPeerStatusChanged(game, statusCode);
            switch (statusCode) {
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
                    {
                        game.SetStrategy(GameState.NebulaGameDisconnected);
                        if (((NetworkGame)game).DisconnectAction == NebulaGameDisconnectAction.ChangeWorld) {
                            game.SetPeer(new GamePeer(game, ConnectionProtocol.Udp));
                            (game as NetworkGame).Connect();
                        }
                        break;
                    }

                default:
                    {
                        game.DebugReturn(DebugLevel.ERROR, statusCode.ToString());
                        break;
                    }
            }
        }

    }
}
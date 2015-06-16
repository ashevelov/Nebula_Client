
using Common;
using ExitGames.Client.Photon;
using Nebula.Mmo.Games;
using System.Collections.Generic;
using UnityEngine;
using Nebula.Mmo.Games.Strategies.Events.Game;
using Nebula.Mmo.Games.Strategies.Events;
using Nebula.Mmo.Games.Strategies.Operations.Game;

namespace Nebula.Mmo.Games.Strategies {

    public class NebulaGameWorkshopEnteredStrategy : DefaultStrategy {


        public NebulaGameWorkshopEnteredStrategy() {

            AddEventHandler((byte)EventCode.WorkshopEntered, new WorkshopEnteredEvent());
            AddEventHandler((byte)EventCode.WorkshopExited, new WorkshopExitedEvent());

            GenericContainerEvent generic = new GenericContainerEvent();
            generic.AddStrategy(CustomEventCode.ShipModelUpdated, new ShipModelUpdatedEvent());
            generic.AddStrategy(CustomEventCode.StationHoldUpdated, new StationHoldUpdatedEvent());
            generic.AddStrategy(CustomEventCode.InventoryUpdated, new InventoryUpdatedEvent());
            generic.AddStrategy(CustomEventCode.PlayerInfoUpdated, new PlayerInfoUpdatedEvent());
            generic.AddStrategy(CustomEventCode.SkillsUpdated, new SkillsUpdatedEvent());
            generic.AddStrategy(CustomEventCode.InventoryItemsAdded, new InventoryItemsAddedEvent());
            AddEventHandler((byte)EventCode.ItemGeneric, generic);

            AddOperationHandler((byte)OperationCode.EnterWorkshop, new EnterWorkshopOperation());

            GenericContainerOperation genericOperation = new GenericContainerOperation();
            genericOperation.AddRPCActionStrategy("GetStation", new GetStationOperation());
            genericOperation.AddRPCActionStrategy("GetWeapon", new GetWeaponOperation());
            genericOperation.AddRPCActionStrategy("GetInventory", new GetInventoryOperation());
            genericOperation.AddRPCActionStrategy("GetPlayerInfo", new GetPlayerInfoOperation());
            genericOperation.AddRPCActionStrategy("GetSkillBinding", new GetSkillBindingOperation());
            genericOperation.AddRPCActionStrategy("GetCombatParams", new GetCombatStatsOperation());

            GenericMultiOperation multi = new GenericMultiOperation();
            genericOperation.AddRPCActionStrategy("MoveItemFromInventoryToStation", multi);
            genericOperation.AddRPCActionStrategy("MoveItemFromStationToInventory", multi);
            genericOperation.AddRPCActionStrategy("TransformObjectAndMoveToHold", multi);
            genericOperation.AddRPCActionStrategy("AddInventorySlots", multi);
            genericOperation.AddRPCActionStrategy("FreeGroup", multi);
            genericOperation.AddRPCActionStrategy("SetLeaderToCharacter", multi);
            genericOperation.AddRPCActionStrategy("VoteForExclude", multi);
            genericOperation.AddRPCActionStrategy("SetGroupOpened", multi);
            genericOperation.AddRPCActionStrategy("RequestOpenedGroups", multi);
            genericOperation.AddRPCActionStrategy("JoinToOpenedGroup", multi);
            genericOperation.AddRPCActionStrategy("CraftItEasy", multi);
            genericOperation.AddRPCActionStrategy("DestroyInventoryItems", multi);
            genericOperation.AddRPCActionStrategy("AddInventoryItem", multi);

            AddOperationHandler((byte)OperationCode.ExecAction, genericOperation);
        }


        public override GameState State {
            get { return GameState.NebulaGameWorkshopEntered; }
        }

        public override void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            switch(statusCode) {
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
                    {
                        var ngame = game as NetworkGame;
                        ngame.SetStrategy(GameState.NebulaGameDisconnected);
                        ngame.ClearItemCache();
                        if (ngame.Avatar != null) {
                            ngame.Avatar.DestroyView();
                            ngame.RemoveAvatar();
                        }
                        break;
                    }
                default:
                    game.DebugReturn(DebugLevel.ERROR, statusCode.ToString());
                    break;
            }
        }
    }
}


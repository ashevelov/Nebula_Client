using Common;
using Game.Network;
using Nebula.Mmo.Games;
using ServerClientCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula {
    public static class NRPC {

        private static bool CheckGameAndPlayer(out NetworkGame game, out MyItem player) {
            game = null;
            player = null;

            game = G.Game;
            if(game == null ) {
                return false;
            }
            player = game.Avatar;
            if(player == null ) {
                return false;
            }
            return true;
        }

        private static bool CheckGame(out NetworkGame game) {
            game = G.Game;
            if(game == null ) {
                return false;
            }
            return true;
        }

        //Request fire from player to target
        public static void RequestFire( ShotType shotType ) {
            NetworkGame game = null;
            MyItem player = null;
            if(false == CheckGameAndPlayer(out game, out player)) {
                Debug.Log("FIRE: check Game And Player Not succedd");
                return;
            }

            if(game.CurrentStrategy != GameState.NebulaGameWorldEntered) {
                Debug.Log("FIRE: check strategy fail");
                return;
            }
            if(false == player.Target.HasTargetAndTargetGameObjectValid) {
                Debug.Log("FIRE: check target fail");
                return;
            }

            Hashtable fireProperties = new Hashtable {
                {(int)SPC.Source, player.Id },
                {(int)SPC.SourceType, player.Type },
                {(int)SPC.Target, player.Target.Item.Id },
                {(int)SPC.TargetType, player.Target.Item.Type },
                {(int)SPC.ShotType, (byte)shotType }
            };

            Operations.RaiseGenericEvent(game, player.Id, player.Type, (byte)CustomEventCode.Fire, fireProperties, (byte)3, EventReceiver.OwnerAndSubscriber);
        }

        public static void RequestUseSkill(int index) {
            NetworkGame game = null;
            MyItem player = null;
            if(false == CheckGameAndPlayer(out game, out player)) {
                return;
            }
            Operations.ExecAction(game, player.Id, "UseSkill", new object[] { index });
        }

        public static void AddToInventory(string containerId, byte containerType, string inventoryObjectId ) {
            NetworkGame game = null;
            MyItem player = null;
            if (false == CheckGameAndPlayer(out game, out player)) {
                return;
            }
            Operations.ExecAction(game, player.Id, "AddFromContainer", new object[] { containerId, containerType, inventoryObjectId });
        }

        public static void TransformInventoryObjectAndMoveToStationHold(InventoryObjectType type, string id) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, string.Empty, "TransformObjectAndMoveToHold", new object[] { type.toByte(), id });
        }

        public static void RequestShipModel() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "RequestShipModel", new object[] { });
        }

        public static void EquipModuleFromHoldToShip(string moduleId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "EquipModule", new object[] { moduleId });
        }

        public static void EquipWeapon(InventoryType inventoryType, string id) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "EquipWeapon", new object[] { (byte)inventoryType, id });
        }

        /// <summary>
        /// Request weapon info from server
        /// </summary>
        public static void RequestWeapon() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetWeapon", new object[] { });
        }

        /// <summary>
        /// Request inventory from server
        /// </summary>
        public static void RequestInventory() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetInventory", new object[] { });
        }

        /// <summary>
        /// Request player info from server
        /// </summary>
        public static void RequestPlayerInfo() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetPlayerInfo", new object[] { });
        }

        /// <summary>
        /// Request world events
        /// </summary>
        /*public static void RequestEvents() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "RequestWorldEvents", new object[] { });
        }*/

        /// <summary>
        /// Request from server current skill binding
        /// </summary>
        public static void RequestSkills() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetSkillBinding", new object[] { });
        }




        /// <summary>
        /// Request move asteroid content to inventory
        /// </summary>
        public static void RequestMoveAsteroidToInventory(string asteroidId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "MoveAsteroidToInventory", new object[] { asteroidId });
        }

        /// <summary>
        /// Move single asteroid content ore item to inventory
        /// </summary>
        public static void RequestMoveAsteroidItemToInventory(string asteroidId, string contentId, byte contentType) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "MoveAsteroidItemToInventory", new object[] { asteroidId, contentId, contentType });
        }

        /// <summary>
        /// Destroy module in station hold and place returned materials into inventory
        /// </summary>
        public static void DestroyModule(string moduleId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "DestroyModule", new object[] { moduleId });
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Increase inventory max slots on 10 units
        /// </summary>
        public static void AddInventorySlots() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "AddInventorySlots", new object[] { });
        }

        /// <summary>
        /// Remove from inventory item
        /// </summary>
        /// <param name="type"></param>
        /// <param name="itemID"></param>
        public static void DestroyInventoryItem(InventoryType inventoryType, InventoryObjectType type, string itemID) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "DestroyInventoryItem", new object[] { inventoryType.toByte(), type.toByte(), itemID });
        }

        public static void DestroyInventoryItems(InventoryType inventoryType, Hashtable inventoryItems) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "DestroyInventoryItems", new object[] { (byte)inventoryType, inventoryItems });
        }

        public static void AddInventoryItem(Hashtable rawItem) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "AddInventoryItem", new object[] { rawItem });
        }

        public static void RequestUserInfo() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetUserInfo", new object[] { game.Engine.LoginGame.GameRefId });
        }

        public static void DeleteCharacter(string characterId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "DeleteCharacter", new object[] { game.Engine.LoginGame.GameRefId, characterId });
        }

        public static void RequestStation() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetStation", new object[] { });
        }

        //public static void SelectCharacter(string characterId, string login, string password) {
        //    NetworkGame game = null;
        //    if (false == CheckGame(out game)) {
        //        return;
        //    }
        //    Operations.SelectCharacter(game, characterId, login, password);
        //}

        //public static void AddCharacter(Race race, Workshop workshop) {
        //    NetworkGame game = null;
        //    if (false == CheckGame(out game)) {
        //        return;
        //    }
        //    Operations.AddCharacter(game, race.toByte(), workshop);
        //}



        /// <summary>
        /// Call activate function on activator in which player touch
        /// </summary>
        /// <param name="activatorId"></param>
        public static void Activate(string activatorId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "OnActivate", new object[] { activatorId });
        }

        ///// <summary>
        ///// Load other world
        ///// </summary>
        ///// <param name="nextWorld"></param>
        //public static void ChangeWorld(string nextWorld) {
        //    NetworkGame game = null;
        //    if (false == CheckGame(out game)) {
        //        return;
        //    }

        //    game.WorldTransition.SetNextWorld(nextWorld);
        //    Operations.ExitWorld(game);
        //}

        public static void ExitToSelectCharacterMenu() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            game.WorldTransition.SetNextWorld(string.Empty);
            Operations.ExitWorld(game);
        }

        public static void SendChatMessage(ChatGroup chatGroup, string message, string receiverLogin) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }

            Hashtable chatMessageProperties = new Hashtable
            {
                {(int)SPC.ChatMessageGroup, (byte)chatGroup},
                {(int)SPC.ChatMessage, message },
                {(int)SPC.ChatSourceLogin, game.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().CharacterName },
                {(int)SPC.ChatSourceName, game.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().CharacterName },
                {(int)SPC.ChatMessageId, System.Guid.NewGuid().ToString() },
                {(int)SPC.ChatReceiverLogin, receiverLogin }
            };
            Operations.RaiseGenericEvent(game, game.AvatarId, ItemType.Avatar.toByte(), (byte)CustomEventCode.ChatMessage, chatMessageProperties, (byte)3, EventReceiver.OwnerAndSubscriber);
        }

        public static void GetChatUpdate() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetChatUpdate", new object[] { });
        }

        public static void RequestShiftDown() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "OnShiftDown", new object[] { });
        }

        public static void RequestShiftUp() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "OnShiftUp", new object[] { });
        }

        /// <summary>
        /// Call rpc generate new weapon and set on model
        /// </summary>
        public static void GenWeapon() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GenWeapon", new object[] { });
        }

        /// <summary>
        /// Try respawn player
        /// </summary>
        public static void Respawn() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }

            Operations.ExecAction(game, game.AvatarId, "Respawn", new object[] { });
        }

        /// <summary>
        /// Request combat params of ship from server(requested in update)
        /// </summary>
        public static void RequestCombatStats() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "GetCombatParams", new object[] { });
        }

        //Operation for demo============================================
        public static void ClearStationHold() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "ClearStationHold", new object[] { });
        }

        public static void ClearInventory() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "ClearInventory", new object[] { });
        }
        public static void RebuildForDemo() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "RebuildForDemo", new object[] { });
        }
        public static void AddDemoSchemes() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "AddDemoSchemes", new object[] { });
        }

        public static void AddDemoModules() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "AddDemoModules", new object[] { });
        }
        public static void DemoPrepare() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "DemoPrepare", new object[] { });
        }

        /// <summary>
        /// Move item from ship inventory to station inventory
        /// </summary>
        public static void MoveItemFromInventoryToStation(InventoryObjectType inventoryObjectType, string inventoryItemId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "MoveItemFromInventoryToStation", new object[] { (byte)inventoryObjectType, inventoryItemId });
        }

        /// <summary>
        /// Move item from station inventory to ship inventory
        /// </summary>
        public static void MoveItemFromStationToInventory(InventoryObjectType inventoryObjectType, string inventoryItemId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "MoveItemFromStationToInventory", new object[] { (byte)inventoryObjectType, inventoryItemId });
        }
        /// <summary>
        /// Test command, which add every ore to inventory by 1 item
        /// </summary>
        public static void CmdAddOres() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "CmdAddOres", new object[] { });
        }


        public static void RequestContainer(string id, ItemType type) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "RequestContainer", new object[] { id, (byte)type });
        }


        public static void AddAllFromContainer(string containerItemId, byte containerItemType) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "AddAllFromContainer", new object[] { containerItemId, containerItemType });
        }


        public static void EA_AddScheme() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "AddScheme", new object[] { });
        }

        public static void TestUseSkill(string skillId) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "TestUseSkill", new object[] { skillId });
        }

        public static void TestRebuild() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "Rebuild", new object[] { });
        }

        public static void TestOut(string str) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "TestOut", new object[] { str });
        }

        public static void GetTargetCombatProperties(string[] properties) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }

            object[] objProperties = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++) {
                objProperties[i] = properties[i];
            }

            object[] callParams = new object[] { objProperties };
            Operations.ExecAction(game, game.AvatarId, "GetTargetCombatProperties", callParams);
        }

        public static void TestApplyTimedBuffToTarget(float durability) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "TestApplyTimedBuffToTarget", new object[] { durability });
        }

        public static void TestGetTimedBuffOnTarget() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "TestGetTimedBuffOnTarget", new object[] { });
        }

        /// <summary>
        /// Add some health to ship
        /// </summary>
        /// <param name="health"></param>
        public static void AddHealth(float health) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "AddHealth", new object[] { health });
        }

        public static void CheckTargetBonus(byte buffType) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "CheckTargetBonus", new object[] { buffType });
        }

        public static void CheckStats() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "CheckStats", new object[] { });
        }

        public static void CollectMemory() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "CollectMemory", new object[] { });
        }

        public static void ServerStats() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "PrintServerStats", new object[] { });
        }

        public static void PlaceStationAtMe() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "PlaceBotAtMe", new object[] { BotItemSubType.PirateStation.toByte() });
        }

        public static void AddExp(int count) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }

            Operations.ExecAction(game, game.AvatarId, "AddExp", new object[] { count });
        }

        public static void SendTestMailFromServerToPlayer(string title, string body) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }

            Operations.ExecAction(game, game.AvatarId, "SendTestMailFromServerToPlayer", new object[] { title, body });
        }

        public static void SendTestMailFromServerToPlayerWithAttachments() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }

            Operations.ExecAction(game, game.AvatarId, "SendTestMailFromServerToPlayerWithAttachments", new object[] { });
        }

        /// <summary>
        /// Send request and adding to player all buffs as Timed Buffs on interval 10 min
        /// </summary>
        public static void TestBuffs() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "TestBuffs", new object[] { });
        }

        public static void SendInviteToGroup(string itemId ) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "SendInviteToGroup", new object[] { itemId });
        }

        public static void ResponseToGroupRequest(bool accept, Hashtable requestInfo ) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "ResponseToGroupRequest", new object[] { accept, requestInfo });
        }

        //Exit me from current group
        public static void ExitFromCurrentGroup() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "ExitFromCurrentGroup", new object[] { });
        }

        //Free group if me is leader of group
        public static void FreeGroup() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "FreeGroup", new object[] { });
        }

        //set group member with character id as leader
        public static void SetLeaderToCharacter(string characterId ) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "SetLeaderToCharacter", new object[] { characterId });
        }

        public static void VoteForExclude(string characterId, string characterDisplayName ) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "VoteForExclude", new object[] { characterId, characterDisplayName });
        }

        /// <summary>
        /// If current user is group leader, than this call change visibility group 
        /// </summary>
        public static void SetGroupOpened(bool opened) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "SetGroupOpened", new object[] { opened });
        }

        /// <summary>
        /// Request list of opened groups from server
        /// </summary>
        public static void RequestOpenedGroups() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "RequestOpenedGroups", new object[] {});
        }

        public static void JoinToOpenedGroup(string groupId ) {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "JoinToOpenedGroup", new object[] { groupId });
        }

        public static void CraftItEasy() {
            NetworkGame game = null;
            if (false == CheckGame(out game)) {
                return;
            }
            Operations.ExecAction(game, game.AvatarId, "CraftItEasy", new object[] { });
        }

        //---------------MASTER OPERATIONS---------------------
        public static void GetServerList(BaseGame game) {
            game.SendOperation((byte)OperationCode.GetServerList, new Dictionary<byte, object>(), true, Settings.ItemChannel);
        }
        //-------------LOGIN OPERATIONS-----------------------------
        public static void Login(BaseGame game, string facebookId, string token, string login) {
            Debug.Log("sending login operation");
            Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                {(byte)ParameterCode.LoginId, facebookId },
                { (byte)ParameterCode.AccessToken, token },
                { (byte)ParameterCode.DisplayName, login}
            };
            game.SendOperation((byte)OperationCode.Login, parameters, true, Settings.ItemChannel);
        }
    }
}


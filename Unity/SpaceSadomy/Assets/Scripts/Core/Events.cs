namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Client.Inventory.Objects;
    using Nebula.Client;
    using Nebula.Mmo.Games;
    using Nebula.UI;

    public static class Events {

        public static event System.Action<GameType, GameState> GameBehaviourChanged;

        public static event System.Action<string, string, Hashtable> ServerItemPropertiesReceived;
        public static event System.Action<StatusCode> ClientStatusCodeChanged;
        public static event System.Action<GameState, GameState> GameStateChanged;
        public static event System.Action<ChatMessage> ChatMessageReceived;
        public static event System.Action<Group> CooperativeGroupUpdated;
        public static event System.Action PlayerCharactersReceived;
        public static event System.Action<IInventoryItemsSource> InventoryItemSourceUpdated;

        /// <summary>
        /// Fire when receive response from Request Search Groups
        /// </summary>
        public static event System.Action SearchGroupResultUpdated;

        //generates when select scheme button clicked in scheme selector crafting view
        public static event System.Action<SchemeInventoryObjectInfo> CraftSchemeSelected;

        //when object transformed and moved to hold on server - generates this event
        public static event System.Action<string> ObjectTransformedAndMovedToHold;

        //when player inventory updated after some action or after request GetInventory response trigger this update inventory event
        public static event System.Action PlayerInventoryUpdated;

        //fires when station hold or station inventory updated
        public static event System.Action StationUpdated;


        /// <summary>
        /// Fired when item removed from NetworkGame.ServerItems
        /// </summary>
        public static event System.Action<ItemType, string> ItemRemoved;

        public static void OnServerItemPropertiesReceived(string itemId, string group, Hashtable properties) {
            if (ServerItemPropertiesReceived != null)
                ServerItemPropertiesReceived(itemId, group, properties);
        }


        public static void OnItemRemoved(ItemType itemType, string itemId) {
            if (ItemRemoved != null)
                ItemRemoved(itemType, itemId);
        }


        public static void OnStatusCodeChanged(StatusCode statusCode) {
            if (ClientStatusCodeChanged != null)
                ClientStatusCodeChanged(statusCode);
        }

        public static void OnGameStateChanged(GameState oldState, GameState newState) {
            if (GameStateChanged != null)
                GameStateChanged(oldState, newState);
        }

        public static void OnChatMessageReceived(ChatMessage msg) {
            if (ChatMessageReceived != null) {
                ChatMessageReceived(msg);
            }
        }


        public static void EvtCraftSchemeSelected(SchemeInventoryObjectInfo scheme) {
            if (CraftSchemeSelected != null) {
                CraftSchemeSelected(scheme);
            }
        }

        public static void EvtObjectTransformedAndMovedToHold(string objId) {
            if (ObjectTransformedAndMovedToHold != null) {
                ObjectTransformedAndMovedToHold(objId);
            }
        }

        public static void EvtPlayerInventoryUpdated() {
            if (PlayerInventoryUpdated != null) {
                PlayerInventoryUpdated();
            }
        }

        public static void EvtStationUpdated() {
            if (StationUpdated != null) {
                StationUpdated();
            }
        }

        public static void EvtCooperativeGroupUpdated(Group group) {
            if(CooperativeGroupUpdated != null ) {
                CooperativeGroupUpdated(group);
            }
        }

        public static void EvtSearchGroupResultUpdated() {
            if(SearchGroupResultUpdated != null) {
                SearchGroupResultUpdated();
            }
        }

        public static void EvtGameBehaviourChanged(GameType gameType, GameState gameState) {
            if(GameBehaviourChanged != null) {
                GameBehaviourChanged(gameType, gameState);
            }
        }

        public static void EvtPlayerCharactersReceived() {
            if(PlayerCharactersReceived != null) {
                PlayerCharactersReceived();
            }
        }

        public static void EvtInventoryItemSourceUpdated(IInventoryItemsSource source) {
            if(InventoryItemSourceUpdated != null) {
                InventoryItemSourceUpdated(source);
            }
        }
    }

}
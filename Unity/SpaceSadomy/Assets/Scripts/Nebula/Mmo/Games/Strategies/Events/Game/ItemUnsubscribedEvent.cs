namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Mmo.Games;
    using Nebula.Mmo.Items;

    public class ItemUnsubscribedEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEventItemUnsubscribed((NetworkGame)game, eventData.Parameters);
        }

        private void HandleEventItemUnsubscribed(NetworkGame game, IDictionary eventData) {

            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            var cameraId = (byte)eventData[(byte)ParameterCode.InterestAreaId];

            Debug.Log("item {0} of type {1} unsibscribed".f(itemId, itemType.toItemType()).Color("orange"));

            Item item;
            if (game.TryGetItem(itemType, itemId, out item)) {
                //if (false == item.IsMine)
                //{
                if (item.RemoveSubscribedInterestArea(cameraId)) {
                    item.RemoveVisibleInterestArea(cameraId);
                }

                //Debug.Log("<color=green>ITEM UNSUBSCRIBED</color>");
                if (item.ExistsView) {
                    //item.Component.ReleaseGUI();
                    Debug.Log(string.Format("item {0}:{1} destroy view", itemId, itemType.toItemType()).Color("orange"));
                    item.DestroyView();
                } else {
                    Debug.Log("EventItemUnsubscribed item not exist view".Color("orange"));
                }

                item.SetSubscribed(false);
                //}

                if(game.Avatar != null ) {
                    if (game.Avatar.Target.TargetId == itemId) {
                        game.Avatar.RequestTarget(string.Empty, (byte)ItemType.Avatar, false);
                        game.Avatar.Target.ResetTarget();
                    }
                }
            } else {
                Debug.Log("EventItemUnsubscribed item in game not found".Color("orange"));
            }


        }
    }

}
namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;


    public class ItemUnsubscribedEventStrategy : IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            HandleEventItemUnsubscribed(game, eventData.Parameters);
        }

        private void HandleEventItemUnsubscribed(NetworkGame game, IDictionary eventData) {

            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            var cameraId = (byte)eventData[(byte)ParameterCode.InterestAreaId];

            //Debug.Log("item {0} of type {1} unsibscribed".f(itemId, itemType.toItemType()));

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
                    item.DestroyView();
                }
                item.SetSubscribed(false);
                //}
            }


        }
    }

}
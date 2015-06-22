namespace Nebula.Mmo.Games.Strategies.Events.Game  {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Mmo.Games;
    using Nebula.Mmo.Items;

    public class ItemMovedEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEventItemMoved((NetworkGame)game, eventData.Parameters);
        }


        private void HandleEventItemMoved(NetworkGame game, IDictionary eventData) {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            Item item;
            if (game.TryGetItem(itemType, itemId, out item)) {
                if (item.IsMine == false) {
                    var position = (float[])eventData[(byte)ParameterCode.Position];
                    var oldPosition = (float[])eventData[(byte)ParameterCode.OldPosition];
                    float[] rotation = eventData.Contains((byte)ParameterCode.Rotation) ? (float[])eventData[(byte)ParameterCode.Rotation] : null;
                    float[] oldRotation = eventData.Contains((byte)ParameterCode.OldRotation) ? (float[])eventData[(byte)ParameterCode.OldRotation] : null;
                    float speed = eventData.Contains(ParameterCode.Speed.toByte()) ? (float)eventData[ParameterCode.Speed.toByte()] : 0f;

                    item.SetPositions(position, oldPosition, rotation, oldRotation, speed);
                }
            }
        }
    }

}
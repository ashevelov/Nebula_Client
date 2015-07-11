namespace Nebula.Mmo.Games.Strategies.Events.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class PlayerStoreUpdateEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            Hashtable hash = eventData.Parameters[(byte)ParameterCode.Info] as Hashtable;

            if(hash == null ) {
                Debug.Log("PlayerStoreUpdateEvent: store hash is null");
                return;
            }

            GameData.instance.store.ParseInfo(hash);
            Debug.LogFormat("credits count = {0}", GameData.instance.store.credits);

        }
    }
}

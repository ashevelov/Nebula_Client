namespace Nebula.Mmo.Games.Strategies.Events.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class GroupUpdateEvent : BaseEventHandler {
        public override void Handle(BaseGame game, EventData eventData) {
            Hashtable groupHash = (Hashtable)eventData.Parameters[(byte)ParameterCode.Group];
            if(groupHash == null ) {
                Debug.LogError("group hash is null");
                return;
            }
            game.Engine.GameData.group.ParseInfo(groupHash);
            Nebula.Events.EvtCooperativeGroupUpdated(game.Engine.GameData.group);

            Debug.LogFormat("Group update event, current group = {0}", game.Engine.GameData.group.groupID);

        }
    }
}
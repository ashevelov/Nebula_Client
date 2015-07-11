namespace Nebula.Mmo.Games.Strategies.Events.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class GroupRemovedEvent : BaseEventHandler {
        public override void Handle(BaseGame game, EventData eventData) {
            string groupID = (string)eventData.Parameters[(byte)ParameterCode.Group];
            if(game.Engine.GameData.group != null ) {
                if(game.Engine.GameData.group.groupID == groupID ) {
                    Debug.Log("Group removed event -> clear");
                    game.Engine.GameData.group.Clear();
                }
            }
        }
    }
}
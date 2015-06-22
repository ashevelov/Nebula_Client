namespace Nebula.Mmo.Games.Strategies.Events.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class GuildUpdateEvent : BaseEventHandler {
        public override void Handle(BaseGame game, EventData eventData) {
            Hashtable guildHash = eventData.Parameters[(byte)ParameterCode.Info] as Hashtable;
            if(guildHash == null ) {
                Debug.Log("GuildUpdateEvent: guild hash is null");
                return;
            }
            game.Engine.GameData.guild.ParseInfo(guildHash);
        }
    }
}

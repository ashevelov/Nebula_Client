namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class SetGuildDescriptionOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("Set guild description error");
                return;
            }
            Debug.Log("set guild description response...");
            Hashtable guildHash = response.Parameters[(byte)ParameterCode.Info] as Hashtable;
            if(guildHash == null ) {
                Debug.Log("SetGuildDescriptionOperation.Handle(): guild hash is null");
                return;
            }

            GameData.instance.guild.ParseInfo(guildHash);
        }
    }
}

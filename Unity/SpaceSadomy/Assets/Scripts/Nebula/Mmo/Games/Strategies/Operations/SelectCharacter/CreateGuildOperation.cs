namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class CreateGuildOperation : BaseOperationHandler {
        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("create guild operation error");
                return;
            }
            Hashtable guildHash = response.Parameters[(byte)ParameterCode.Info] as Hashtable;
            if(guildHash == null ) {
                Debug.Log("guild hash null");
                return;
            }
            game.Engine.GameData.guild.ParseInfo(guildHash);
        }
    }
}

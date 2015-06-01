using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Nebula.Client;

namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    public class GetCharactersOperation : BaseOperationHandler {

        public override void Handle(BaseGame game, OperationResponse response) {
            if(response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogErrorFormat("error, returc code = {0}, debug message = {1}", (ReturnCode)response.ReturnCode, response.DebugMessage);
                return;
            }

            Hashtable characterHash = response.Parameters[(byte)ParameterCode.Characters] as Hashtable;

            ClientPlayerCharactersContainer playerCharacters = new ClientPlayerCharactersContainer(characterHash);
            game.Engine.OnPlayerCharactersReceived(playerCharacters);
        }
    }
}

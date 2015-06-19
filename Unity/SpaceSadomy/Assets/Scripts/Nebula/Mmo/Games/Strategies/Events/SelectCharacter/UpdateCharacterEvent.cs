namespace Nebula.Mmo.Games.Strategies.Events.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Client;

    public class UpdateCharacterEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            Hashtable characterHash = eventData.Parameters[(byte)ParameterCode.Characters] as Hashtable;
            if (characterHash == null) {
                Debug.Log("Character hash is null");
                return;
            }

            var player = new ClientPlayerCharactersContainer(characterHash);
            game.Engine.OnPlayerCharactersReceived(player);
            Debug.Log("Update Player Characters Event received");
        }
    }
}
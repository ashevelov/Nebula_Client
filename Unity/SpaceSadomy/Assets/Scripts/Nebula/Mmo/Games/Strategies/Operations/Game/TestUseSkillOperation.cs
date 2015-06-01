using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class TestUseSkillOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            Debug.Log("TestUseSkill()".Color("orange").Bold());
            foreach (DictionaryEntry entry in Result(response))
            {
                Debug.Log(string.Format("{0}:{1}", entry.Key, entry.Value));
            }
        }
    }

}
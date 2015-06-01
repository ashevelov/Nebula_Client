using Common;
using ExitGames.Client.Photon;
using Game.Network;
using Game.Space;
using System.Collections;
using UnityEngine;
using Nebula;
using Nebula.Mmo.Games;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GetBonusesOnTargetOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            Debug.Log("GetBonusesOnTarget()".Color("orange").Bold());
            foreach (DictionaryEntry entry in Result(response))
            {
                Debug.Log(string.Format("{0}:{1}", (BonusType)entry.Key, entry.Value));
            }
        }
    }
}

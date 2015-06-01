using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula.Mmo.Games.Strategies {
    public class BaseOperationHandler {

        public virtual void Handle(BaseGame game, OperationResponse response) {
            Debug.LogFormat("Handler at game {0} for operation {1}", game.GameType, response.OperationCode);
        }
    }
}

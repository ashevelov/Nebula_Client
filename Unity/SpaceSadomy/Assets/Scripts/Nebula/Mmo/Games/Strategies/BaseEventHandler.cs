using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula.Mmo.Games.Strategies {
    public class BaseEventHandler {

        public virtual void Handle(BaseGame game, EventData eventData) {
            Debug.LogFormat("Event handler for game {0} from event {1}", game.GameType, eventData.Code);
        }
    }
}
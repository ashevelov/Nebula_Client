namespace Nebula.Mmo.Games.Strategies {
    using UnityEngine;
    using System.Collections;
    using System;

    public class SelectCharacterDisconnectedStrategy : DefaultStrategy {

        public override GameState State {
            get {
                return GameState.SelectCharacterDisconnected;
            }
        }
    }
}

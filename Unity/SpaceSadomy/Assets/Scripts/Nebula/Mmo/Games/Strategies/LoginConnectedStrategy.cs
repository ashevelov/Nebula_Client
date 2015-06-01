using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using Common;
using System.Collections.Generic;
using Nebula.Mmo.Games.Strategies.Operations.Login;

namespace Nebula.Mmo.Games.Strategies {
    public class LoginConnectedStrategy : DefaultStrategy {

        public override GameState State {
            get {
                return GameState.LoginConnected;
            }
        }
 
        public LoginConnectedStrategy() {
            AddOperationHandler((byte)OperationCode.Login, new LoginOperation());
        }
    }
}

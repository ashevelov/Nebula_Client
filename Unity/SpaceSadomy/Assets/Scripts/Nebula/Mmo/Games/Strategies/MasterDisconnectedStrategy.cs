﻿using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;

namespace Nebula.Mmo.Games.Strategies {

    public class MasterDisconnectedStrategy : DefaultStrategy {
        public override GameState State {
            get {
                return GameState.MasterDisconnected;
            }
        }
    }
}

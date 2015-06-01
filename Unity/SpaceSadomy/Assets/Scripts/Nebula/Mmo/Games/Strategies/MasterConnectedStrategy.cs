using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using Common;
using Nebula.Mmo.Games.Strategies.Operations.Master;

namespace Nebula.Mmo.Games.Strategies {
    public class MasterConnectedStrategy : DefaultStrategy {

        public MasterConnectedStrategy() : base () {
            AddOperationHandler((byte)OperationCode.GetServerList, new GetServerListOperation());
        }
        public override GameState State {
            get {
                return GameState.MasterConnected;
            }
        }
    }
}
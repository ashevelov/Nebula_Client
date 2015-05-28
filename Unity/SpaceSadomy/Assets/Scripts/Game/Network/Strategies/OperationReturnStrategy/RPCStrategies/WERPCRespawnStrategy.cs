﻿using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula {
    public class WERPCRespawnStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            G.Game.Ship.SetNeedRespawnFlag();
        }
    }
}
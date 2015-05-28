using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula
{
    public abstract class OperationReturnStrategy
    {
        public virtual void Handle(NetworkGame game, OperationResponse response)
        {

        }
    }
}

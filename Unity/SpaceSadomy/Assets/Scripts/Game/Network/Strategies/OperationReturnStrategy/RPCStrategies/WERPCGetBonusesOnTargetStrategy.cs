using Common;
using ExitGames.Client.Photon;
using Game.Network;
using Game.Space;
using System.Collections;
using UnityEngine;
using Nebula;

namespace Nebula
{
    public class WERPCGetBonusesOnTargetStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            Debug.Log("GetBonusesOnTarget()".Color("orange").Bold());
            foreach (DictionaryEntry entry in Result(response))
            {
                Debug.Log(string.Format("{0}:{1}", (BonusType)entry.Key, entry.Value));
            }
        }
    }
}

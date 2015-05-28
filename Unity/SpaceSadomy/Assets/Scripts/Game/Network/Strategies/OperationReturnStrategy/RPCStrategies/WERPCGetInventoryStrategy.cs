using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Space;
using Game.Network;
using Nebula.Client.Inventory;

namespace Nebula
{
    public class WERPCGetInventoryStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            ClientInventory newInventory = new ClientInventory(Result(response));
            game.Inventory.Replace(newInventory);
            Events.EvtPlayerInventoryUpdated();
        }
    }
}

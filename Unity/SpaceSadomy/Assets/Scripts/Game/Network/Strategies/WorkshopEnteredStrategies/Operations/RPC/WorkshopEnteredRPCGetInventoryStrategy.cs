using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Space;
using Nebula.Client.Inventory;

namespace Nebula {
    public class WorkshopEnteredRPCGetInventoryStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            ClientInventory inventory = new ClientInventory(Result(response));
            game.Inventory.Replace(inventory);
            Events.EvtPlayerInventoryUpdated();
        }
    }
}

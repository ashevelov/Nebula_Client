using ExitGames.Client.Photon;
using Nebula.Client.Inventory;
using UnityEngine;
using System.Text;
using Common;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GetInventoryOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            Debug.Log("GetInventory response");
            StringBuilder builder = new StringBuilder();
            CommonUtils.ConstructHashString(Result(response), 1, ref builder);
            Debug.Log(builder.ToString());

            ClientInventory newInventory = new ClientInventory(Result(response));
            Debug.Log("new inventory count: " + newInventory.OrderedItems().Count);
            game.Inventory.Replace(newInventory);
            global::Nebula.Events.EvtPlayerInventoryUpdated();

            Debug.Log("Inventory count: " + game.Inventory.OrderedItems().Count);
            
        }
    }
}

using Common;
using ExitGames.Client.Photon;
using System.Collections;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class AttachInterestAreaOperation : BaseOperationHandler
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            this.HandleEventInterestAreaAttached((NetworkGame)game, response.Parameters);
        }


        private void HandleEventInterestAreaAttached(NetworkGame game, IDictionary eventData)
        {
            var itemType = (byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];

            game.OnCameraAttached(itemId, itemType);
        }
    }
}

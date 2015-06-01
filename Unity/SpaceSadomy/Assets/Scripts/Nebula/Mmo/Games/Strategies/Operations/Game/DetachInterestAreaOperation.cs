using ExitGames.Client.Photon;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class DetachInterestAreaOperation : BaseOperationHandler
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleEventInterestAreaDetached((NetworkGame)game);
        }

        private void HandleEventInterestAreaDetached(NetworkGame game)
        {
            game.OnCameraDetached();
        }
    }
}
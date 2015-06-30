using ExitGames.Client.Photon;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GetPlayerInfoOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }
        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            GameData.instance.playerInfo.ParseInfo(Result(response));
        }
    }
}

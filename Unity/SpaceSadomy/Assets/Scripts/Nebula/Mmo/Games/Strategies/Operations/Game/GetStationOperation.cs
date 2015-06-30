
using ExitGames.Client.Photon;
using Nebula.Mmo.Games;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GetStationOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            GameData.instance.station.LoadInfo(Result(response));
        }
    }
}

using ExitGames.Client.Photon;
using Game.Network;
using Nebula.Mmo.Games;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GetBonusesOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            game.Bonuses.Replace(Result(response));
        }
    }
}

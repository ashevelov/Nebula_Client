using ExitGames.Client.Photon;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GetWeaponOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            GameData.instance.ship.Weapon.ParseInfo(Result(response));
        }
    }
}

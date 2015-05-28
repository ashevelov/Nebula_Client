using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula {
    public class WERPCGetWeaponStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.OnWeaponReceived(Result(response));
        }
    }
}


using ExitGames.Client.Photon;

namespace Nebula {
    public class WorkshopEnteredRPCGetWeaponStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.OnWeaponReceived(Result(response));
        }
    }
}

using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula
{
    public class WERPCGetBonusesStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.Bonuses.Replace(Result(response));
        }
    }
}

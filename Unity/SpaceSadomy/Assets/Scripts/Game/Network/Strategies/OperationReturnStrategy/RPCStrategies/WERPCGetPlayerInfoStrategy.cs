using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula
{
    public class WERPCGetPlayerInfoStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.PlayerInfo.ParseInfo(Result(response));
        }
    }
}

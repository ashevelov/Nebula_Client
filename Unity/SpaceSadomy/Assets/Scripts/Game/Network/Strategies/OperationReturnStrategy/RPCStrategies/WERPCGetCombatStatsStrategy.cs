using ExitGames.Client.Photon;
using Game.Network;

namespace Nebula
{
    public class WERPCGetCombatStatsStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            G.Game.CombatStats.ParseInfo(Result(response));
        }
    }
}

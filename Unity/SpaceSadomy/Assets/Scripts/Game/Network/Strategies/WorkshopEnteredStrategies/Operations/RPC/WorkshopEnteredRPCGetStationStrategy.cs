
using ExitGames.Client.Photon;

namespace Nebula {
    public class WorkshopEnteredRPCGetStationStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
        {
            game.Station.LoadInfo(Result(response));
        }
    }
}

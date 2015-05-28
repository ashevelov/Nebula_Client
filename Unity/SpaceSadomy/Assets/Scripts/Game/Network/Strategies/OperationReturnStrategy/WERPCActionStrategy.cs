using Common;
using ExitGames.Client.Photon;
using System.Collections.Generic;

namespace Nebula {
    public class WERPCActionStrategy : OperationReturnStrategy
    {
        private Dictionary<string, RPCOperationReturnStrategy> actionStrategies = new Dictionary<string, RPCOperationReturnStrategy>();

        public override void Handle(NetworkGame game, OperationResponse response)
        {
            string action = Action(response);
            if (this.actionStrategies.ContainsKey(action))
            {
                this.actionStrategies[action].Handle(game, response);
            }
        }

        private string Action(OperationResponse response)
        {
            return (string)response[(byte)ParameterCode.Action];
        }

        public void AddRPCActionStrategy(string action, RPCOperationReturnStrategy strategy)
        {
            this.actionStrategies.Add(action, strategy);
        }
    }
}
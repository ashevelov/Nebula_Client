using Common;
using ExitGames.Client.Photon;
using Nebula.Mmo.Games;
using System.Collections.Generic;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GenericContainerOperation : BaseOperationHandler
    {
        private Dictionary<string, BaseGenericOperation> actionStrategies = new Dictionary<string, BaseGenericOperation>();

        public override void Handle(BaseGame game, OperationResponse response) {
            string action = Action(response);
            if (this.actionStrategies.ContainsKey(action)) {
                this.actionStrategies[action].Handle(game, response);
            }
        }

        private string Action(OperationResponse response)
        {
            return (string)response[(byte)ParameterCode.Action];
        }

        public void AddRPCActionStrategy(string action, BaseGenericOperation strategy)
        {
            this.actionStrategies.Add(action, strategy);
        }
    }
}
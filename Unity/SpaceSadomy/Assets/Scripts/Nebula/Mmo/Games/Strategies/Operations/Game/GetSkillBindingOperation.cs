using ExitGames.Client.Photon;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class GetSkillBindingOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            ((NetworkGame)game).Skills.ParseInfo(Result(response));
        }
    }
}

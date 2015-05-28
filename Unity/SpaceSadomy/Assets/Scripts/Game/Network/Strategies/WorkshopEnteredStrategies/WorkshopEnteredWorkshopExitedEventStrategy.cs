using Common;
using ExitGames.Client.Photon;

namespace Nebula {
    public class WorkshopEnteredWorkshopExitedEventStrategy : IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            if (HangarShip.Get) {
                HangarShip.Get.Stop();
            }
            game.OnWorkshopExited(Position(eventData));
        }

        private float[] Position(EventData eventData) {
            return (float[])eventData.Parameters[ParameterCode.Position.toByte()];
        }
    }
}
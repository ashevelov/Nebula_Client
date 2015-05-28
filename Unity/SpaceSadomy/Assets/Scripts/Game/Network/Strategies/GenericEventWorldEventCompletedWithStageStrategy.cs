namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Game.Network;

    public class GenericEventWorldEventCompletedWithStageStrategy : GenericEventStrategy, IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            game.SetEvent(Properties(eventData));

            if (Properties(eventData).GetValue<string>(GenericEventProps.id, string.Empty) == "EV0003") {
                var evt = game.GetEvent(Properties(eventData).GetValue<string>(GenericEventProps.WorldId, string.Empty), "EV0003");
                if (evt != null) {
                    Debug.Log("E3 completed on stage: {0}".f(evt.Stage.StageId));
                }
            }
        }
    }

}
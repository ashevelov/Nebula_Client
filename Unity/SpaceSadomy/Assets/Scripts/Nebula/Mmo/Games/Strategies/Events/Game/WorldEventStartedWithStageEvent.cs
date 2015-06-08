/*
namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using ServerClientCommon;
    using UnityEngine;

    public class WorldEventStartedWithStageEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            game.SetEvent(Properties(eventData));

            if (Properties(eventData).GetValue<string>((int)SPC.Id, string.Empty) == "EV0003") {
                var evt = game.GetEvent(Properties(eventData).GetValue<string>((int)SPC.WorldId, string.Empty), "EV0003");
                if (evt != null) {
                    Debug.Log("E3 started on stage: {0}".f(evt.Stage.StageId));
                }
            }
        }
    }

}*/

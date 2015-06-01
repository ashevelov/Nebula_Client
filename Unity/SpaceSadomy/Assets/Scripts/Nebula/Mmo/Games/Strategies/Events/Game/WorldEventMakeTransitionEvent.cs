namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using ServerClientCommon;
    using UnityEngine;

    public class WorldEventMakeTransitionEvent : BaseGenericEvent {
        private readonly StringSubCache<string> stringCache = new StringSubCache<string>();

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            if (string.IsNullOrEmpty(TransitionText(eventData))) {
                Debug.Log("Transition text is empty");
                return;
            }

            G.Game.Engine.GameData.Chat.PastLocalMessage(stringCache.String(TransitionText(eventData), TransitionText(eventData)));

            if (Properties(eventData).GetValue<string>((int)SPC.Id, string.Empty) == "EV0003") {
                var evt = game.GetEvent(Properties(eventData).GetValue<string>((int)SPC.WorldId, string.Empty), "EV0003");
                if (evt != null) {
                    Debug.Log("E3 make transition to stage: {0}".f(evt.Stage.StageId));
                }
            }
        }

        private string TransitionText(EventData eventData) {
            return Properties(eventData).GetValue<string>("transition_text", string.Empty);
        }
    }

}
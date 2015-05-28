namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class GenericEventWorldEventMakeTransitionStrategy : GenericEventStrategy, IServerEventStrategy {
        private readonly StringSubCache<string> stringCache = new StringSubCache<string>();

        public void Handle(NetworkGame game, EventData eventData) {
            if (string.IsNullOrEmpty(TransitionText(eventData))) {
                Debug.Log("Transition text is empty");
                return;
            }

            G.Game.Chat.PastLocalMessage(stringCache.String(TransitionText(eventData), TransitionText(eventData)));

            if (Properties(eventData).GetValue<string>(GenericEventProps.id, string.Empty) == "EV0003") {
                var evt = game.GetEvent(Properties(eventData).GetValue<string>(GenericEventProps.WorldId, string.Empty), "EV0003");
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
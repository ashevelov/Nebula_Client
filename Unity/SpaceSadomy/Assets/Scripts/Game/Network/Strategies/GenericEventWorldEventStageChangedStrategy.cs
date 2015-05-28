namespace Nebula {
    using UnityEngine;
    using ExitGames.Client.Photon;
    using Common;


    public class GenericEventWorldEventStageChangedStrategy : GenericEventStrategy, IServerEventStrategy {
        private const string EVENT_MARK_PREFAB_PATH = "Prefabs/Event_Mark";

        private readonly StringSubCache<string> stringCache = new StringSubCache<string>();

        public void Handle(NetworkGame game, EventData eventData) {
            game.WorldEventConnection.SetEvent(Properties(eventData));

            if (string.IsNullOrEmpty(EventId(eventData))) {
                Debug.Log("Event id is null|empty");
                return;
            }
            if (string.IsNullOrEmpty(WorldId(eventData))) {
                Debug.Log("World id is null|empty");
                return;
            }

            var eventObject = game.GetEvent(WorldId(eventData), EventId(eventData));

            if (eventObject == null) {
                Debug.Log("Event not founded: {0}".f(EventId(eventData)));
                return;
            }

            var stageObject = eventObject.Stage;

            if (stageObject == null) {
                Debug.Log("Stage not founded");
                return;
            }

            string stageTextId = stageObject.StartTextId;

            if (string.IsNullOrEmpty(stageTextId)) {
                Debug.Log("Stage don't have start text");
                return;
            }

            string resourceText = stringCache.String(stageTextId, stageTextId);
            resourceText = resourceText.ReplaceVariables(eventObject.VariablesInfo);

            G.Game.Chat.PastLocalMessage(resourceText);
            GameObject.Instantiate(PrefabCache.Get(EVENT_MARK_PREFAB_PATH), eventObject.Position.toVector(), Quaternion.identity);

            if (eventObject.Id == "EV0003") {
                Debug.Log("E3 stage changed to {0}".f(stageObject.StageId));
            }
        }

        private string EventId(EventData eventData) {
            return Properties(eventData).GetValue<string>(GenericEventProps.id, string.Empty);
        }

        private string WorldId(EventData eventData) {
            return Properties(eventData).GetValue<string>(GenericEventProps.WorldId, string.Empty);
        }


    }

}
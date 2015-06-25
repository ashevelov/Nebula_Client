namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using ServerClientCommon;
    using Nebula.Client.Res;
    using Nebula.Resources;

    public class GameEventStatusEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            NetworkGame ngame = game as NetworkGame;
            Hashtable gameEventParameters = Data(eventData) as Hashtable;
            string eventId = (string)gameEventParameters[(int)SPC.Id];
            bool active = (bool)gameEventParameters[(int)SPC.Active];

            if(ngame.ClientWorld.Id != MmoEngine.Get.GameData.World.Name) {
                Debug.LogErrorFormat("client world = {0} and game data world = {1} don't same", ngame.ClientWorld.Id, MmoEngine.Get.GameData.World.Name);
            }
            Debug.LogFormat("received status of game event in world = {0}, game event id = {1}", MmoEngine.Get.GameData.World.Name, eventId);

            ResGameEventData gameEventData;
            if(DataResources.Instance.gameEvents.TryGetEvent(MmoEngine.Get.GameData.World.Name, eventId, out gameEventData)) {
                //if(active) {
                //    MmoEngine.Get.GameData.Chat.PastLocalMessage(string.Format("Started: {0}", StringCache.Get(gameEventData.descriptionId)));
                //} else {
                //    MmoEngine.Get.GameData.Chat.PastLocalMessage(string.Format("Completed: {0}", StringCache.Get(gameEventData.descriptionId)));
                //}
            } else {
                Debug.LogErrorFormat("not found event {0} at world {1}", eventId, MmoEngine.Get.GameData.World.Name);
            }
           
        }
    }
}

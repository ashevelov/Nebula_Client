using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Nebula.Mmo.Games;

namespace Nebula.Mmo.Games.Strategies.Events.Game {
    public class WorkshopEnteredEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData)
        {
            if(Info(eventData) == null )
            {
                Debug.LogError("station data is null...");
            }
            GameData.instance.station.LoadInfo(Info(eventData));

            if (game.Avatar != null && game.Avatar.ExistsView)
            {
                game.Avatar.DestroyView();
            }
            game.ClearItemCache();
            game.RemoveAvatar();
            //game.ClearCameras();

            if (WorkshopType(eventData) == WorkshopStrategyType.Angar) {

                //Application.LoadLevel("Angar");
                LoadScenes.Load("Angar");

            } else if (WorkshopType(eventData) == WorkshopStrategyType.Planet) {
                if (GameData.instance.clientWorld.Zone.Id == "E7") {
                    Application.LoadLevel("Demo - Colorized Red 1");
                } else {
                    Application.LoadLevel("Demo - Colorized Red");
                }
            }
        }

        private Hashtable Info(EventData eventData)
        {
            return (Hashtable)eventData.Parameters[ParameterCode.Info.toByte()];
        }

        private WorkshopStrategyType WorkshopType(EventData eventData)
        {
            return (WorkshopStrategyType)(byte)eventData.Parameters[ParameterCode.Type.toByte()];
        }
    }
}
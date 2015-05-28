using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;

namespace Nebula
{
    public class WorkshopEnteredEventStrategy : IServerEventStrategy
    {

        public void Handle(NetworkGame game, EventData eventData)
        {
            if(Info(eventData) == null )
            {
                Debug.LogError("station data is null...");
            }
            game.Station.LoadInfo(Info(eventData));

            if (game.Avatar != null && game.Avatar.ExistsView)
            {
                game.Avatar.DestroyView();
            }
            game.ClearItemCache();
            game.DeleteAvatar();
            //game.ClearCameras();

            if (WorkshopType(eventData) == WorkshopStrategyType.Angar)
                Application.LoadLevel("Angar");
            else if (WorkshopType(eventData) == WorkshopStrategyType.Planet)
            {
                if (G.Game.ClientWorld.Zone.Id == "E7")
                {
                    Application.LoadLevel("Demo - Colorized Red 1");
                }
                else
                {
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
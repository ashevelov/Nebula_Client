using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula
{
    public class WorkshopEnteredPlayerInfoUpdatedGenericEventStrategy : GenericEventStrategy, IServerEventStrategy 
    {

        public void Handle(NetworkGame game, EventData eventData)
        {
            if(Properties(eventData) == null )
            {
                Debug.LogError("PlayerInfoUpdated generic event properties null...");
                return;
            }

            G.Game.PlayerInfo.ParseInfo(Properties(eventData));
        }
    }
}

using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula
{
    public class WorkshopEnteredShipModelUpdatedItemGenericEventStrategy : GenericEventStrategy, IServerEventStrategy 
    {

        public void Handle(NetworkGame game, EventData eventData)
        {
            NetworkGame.OnShipModelUpdated(game, Properties(eventData));
        }
    }
}
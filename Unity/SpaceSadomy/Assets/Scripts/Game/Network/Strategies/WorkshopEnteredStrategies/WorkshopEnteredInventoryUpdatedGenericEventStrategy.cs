using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Nebula.Client.Inventory;
using Nebula.UI;

namespace Nebula
{
    public class WorkshopEnteredInventoryUpdatedGenericEventStrategy : GenericEventStrategy, IServerEventStrategy 
    {

        public void Handle(NetworkGame game, EventData eventData)
        {
            NetworkGame.OnInventoryUpdated(game, Properties(eventData));
            Events.EvtPlayerInventoryUpdated();
        }
    }

}

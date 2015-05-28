using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula
{
    public class WorkshopEnteredSkillsUpdatedGenericEventStrategy : GenericEventStrategy, IServerEventStrategy 
    {

        public void Handle(NetworkGame game, EventData eventData)
        {
            if(Properties(eventData) == null)
            {
                Debug.LogError("Skills null in SkillsUpdatedEvent...");
                return;
            }
            G.Game.Skills.ParseInfo(Properties(eventData));
        }
    }
}
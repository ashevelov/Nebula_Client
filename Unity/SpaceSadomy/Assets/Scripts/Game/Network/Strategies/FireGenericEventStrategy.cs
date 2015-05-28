using Common;
using ExitGames.Client.Photon;
using Game.Network;
using System.Collections.Generic;
using UnityEngine;
using Nebula;
using Nebula.Client;

public class FireGenericEventStrategy : GenericEventStrategy, IServerEventStrategy 
{
    private readonly StringSubCache<string> strings = new StringSubCache<string>();

    public void Handle(NetworkGame game, EventData eventData)
    {
        if(!ShotAllowed(eventData))
        {
            PrintError(eventData);
            return;
        }
        if(ShotBlocked(eventData))
        {
            PrintError(eventData);
            return;
        }
        game.OnItemFire(Properties(eventData));
    }



    private bool ShotAllowed(EventData eventData)
    {
        return Properties(eventData).GetValue<bool>(GenericEventProps.fire_allowed, false);
    }

    private bool ShotBlocked(EventData eventData)
    {
        return Properties(eventData).GetValue<bool>(GenericEventProps.fire_blocked, true);
    }

    private string ErrorMessageId(EventData eventData)
    {
        return Properties(eventData).GetValue<string>(GenericEventProps.error_message_id, string.Empty);
    }

    private string SourceId(EventData eventData)
    {
        return Properties(eventData).GetValue<string>(GenericEventProps.source_id, string.Empty);
    }

    private void PrintError(EventData eventData)
    {
        if(!string.IsNullOrEmpty(ErrorMessageId(eventData)))
        {
            if(G.PlayerItem == null )
            {
                return;
            }

            if(G.PlayerItem.Id != SourceId(eventData))
            {
                return;
            }

            var list = new List<string> { strings.String(ErrorMessageId(eventData), ErrorMessageId(eventData)).Trim() };
            G.Game.Chat.PastLocalMessage(list.ToNewLineSeparatedString());
        }
    }
}

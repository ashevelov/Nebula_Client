namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Client.Mail;
    using Nebula.Mmo.Games;
    using Nebula.UI;
    using ServerClientCommon;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MailBoxUpdatedEvent : BaseGenericEvent {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            Hashtable mailBoxInfo = Properties(eventData);
            G.Game.MailBox().Replace(new ClientMailBox(mailBoxInfo));
            Debug.Log("<color=orange>Mail message received</color>" + G.Game.MailBox().ToString());
        }
    }


}
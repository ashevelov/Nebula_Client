namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Client;
    using Nebula.Mmo.Games;
    using Nebula.Resources;
    using ServerClientCommon;
    using System.Collections.Generic;

    public class FireEvent : BaseGenericEvent {
        private readonly StringSubCache<string> strings = new StringSubCache<string>();

        public override void Handle(BaseGame game, EventData eventData) {
            if (!ShotAllowed(eventData)) {
                PrintError(eventData);
                return;
            }
            if (ShotBlocked(eventData)) {
                PrintError(eventData);
                return;
            }
            ((NetworkGame)game).OnItemFire(Properties(eventData));
        }

        private bool ShotAllowed(EventData eventData) {
            return Properties(eventData).GetValue<bool>((int)SPC.FireAllowed, false);
        }

        private bool ShotBlocked(EventData eventData) {
            return Properties(eventData).GetValue<bool>((int)SPC.FireBlocked, true);
        }

        private string ErrorMessageId(EventData eventData) {
            return Properties(eventData).GetValue<string>((int)SPC.ErrorMessageId, string.Empty);
        }

        private string SourceId(EventData eventData) {
            return Properties(eventData).GetValue<string>((int)SPC.Source, string.Empty);
        }

        private void PrintError(EventData eventData) {
            if (!string.IsNullOrEmpty(ErrorMessageId(eventData))) {
                if (G.PlayerItem == null) {
                    return;
                }

                if (G.PlayerItem.Id != SourceId(eventData)) {
                    return;
                }

                var list = new List<string> { strings.String(ErrorMessageId(eventData), ErrorMessageId(eventData)).Trim() };
                //G.Game.Engine.GameData.Chat.PastLocalMessage(list.ToNewLineSeparatedString());
            }
        }
    }
}
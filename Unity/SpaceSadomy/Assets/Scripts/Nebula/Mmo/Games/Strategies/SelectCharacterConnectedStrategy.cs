namespace Nebula.Mmo.Games.Strategies {
    using Common;
    using Nebula.Mmo.Games.Strategies.Events.SelectCharacter;
    using Nebula.Mmo.Games.Strategies.Operations.SelectCharacter;

    public class SelectCharacterConnectedStrategy : DefaultStrategy {

        public SelectCharacterConnectedStrategy() {
            AddEventHandler((byte)SelectCharacterEventCode.NotificationUpdate, new UpdateNotificationsEvent());
            AddEventHandler((byte)SelectCharacterEventCode.CharactersUpdate, new UpdateCharacterEvent());
            AddEventHandler((byte)SelectCharacterEventCode.GuildUpdate, new GuildUpdateEvent());
            AddEventHandler((byte)SelectCharacterEventCode.ChatMessageEvent, new ChatMessageEvent());
            AddEventHandler((byte)SelectCharacterEventCode.GroupUpdateEvent, new GroupUpdateEvent());
            AddEventHandler((byte)SelectCharacterEventCode.GroupRemovedEvent, new GroupRemovedEvent());

            //operation handlers
            AddOperationHandler((byte)SelectCharacterOperationCode.GetCharacters, new GetCharactersOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.CreateCharacter, new CreateCharacterOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.DeleteCharacter, new DeleteCharacterOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.SelectCharacter, new SelectCharacterOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.GetMails, new GetMailsOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.WriteMailMessage, new WriteMailMessageOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.DeleteMailMessage, new DeleteMailMessageOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.DeleteAttachment, new DeleteAttachmentOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.InvokeMethod, new InvokeMethodOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.GetNotifications, new GetNotificationsOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.HandleNotification, new HandleNotificationOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.GetGuild, new GetGuildOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.CreateGuild, new CreateGuildOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.InviteToGuild, new InviteGuildOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.ExitGuild, new ExitGuildOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.SetGuildDescription, new SetGuildDescriptionOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.ChangeGuildMemberStatus, new ChangeGuildMemberStatusOperation());
        }

        public override GameState State {
            get {
                return GameState.SelectCharacterConnected;
            }
        }
    }
}
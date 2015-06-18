namespace Nebula.Mmo.Games.Strategies {
    using Common;
    using Nebula.Mmo.Games.Strategies.Events.SelectCharacter;
    using Nebula.Mmo.Games.Strategies.Operations.SelectCharacter;

    public class SelectCharacterConnectedStrategy : DefaultStrategy {

        public SelectCharacterConnectedStrategy() {
            AddEventHandler((byte)SelectCharacterEventCode.NotificationUpdate, new UpdateNotificationsEvent());
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

        }

        public override GameState State {
            get {
                return GameState.SelectCharacterConnected;
            }
        }
    }
}
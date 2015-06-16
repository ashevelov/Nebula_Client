namespace Nebula.Mmo.Games.Strategies {
    using Common;
    using Nebula.Mmo.Games.Strategies.Operations.SelectCharacter;

    public class SelectCharacterConnectedStrategy : DefaultStrategy {

        public SelectCharacterConnectedStrategy() {
            AddOperationHandler((byte)SelectCharacterOperationCode.GetCharacters, new GetCharactersOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.CreateCharacter, new CreateCharacterOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.DeleteCharacter, new DeleteCharacterOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.SelectCharacter, new SelectCharacterOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.GetMails, new GetMailsOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.WriteMailMessage, new WriteMailMessageOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.DeleteMailMessage, new DeleteMailMessageOperation());
            AddOperationHandler((byte)SelectCharacterOperationCode.DeleteAttachment, new DeleteAttachmentOperation());

        }

        public override GameState State {
            get {
                return GameState.SelectCharacterConnected;
            }
        }
    }
}
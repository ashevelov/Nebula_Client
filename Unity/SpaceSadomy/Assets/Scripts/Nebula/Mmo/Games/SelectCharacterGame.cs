namespace Nebula.Mmo.Games {
    using Common;
    using Nebula.Client;
    using Nebula.Client.Inventory;
    using Nebula.Mmo.Games.Strategies;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SelectCharacterGame : BaseGame {

        private static SelectCharacterGame instance = null;

        public static SelectCharacterGame Instance() {
            if(instance == null ) {
                instance = MmoEngine.Get.SelectCharacterGame;
            }
            return instance;
        }

        private ClientPlayerCharactersContainer playerCharacters;

        public SelectCharacterGame(MmoEngine engine, Settings settings)
            : base(engine, settings) {
            playerCharacters = new ClientPlayerCharactersContainer();

            strategies = new Dictionary<GameState, Strategies.IGameStrategy> {
                {GameState.SelectCharacterDisconnected, new SelectCharacterDisconnectedStrategy() },
                {GameState.SelectCharacterWaitingConnect, new SelectCharacterWaitingConnectStrategy() },
                { GameState.SelectCharacterConnected, new SelectCharacterConnectedStrategy() }
            };
            SetStrategy(GameState.SelectCharacterDisconnected);
        }

        public override void Connect(string ipAddress, int port, string application) {
            SetStrategy(GameState.SelectCharacterWaitingConnect);
            base.Connect(ipAddress, port, application);
        }

        public override GameType GameType {
            get {
                return GameType.SelectCharacter;
            }
        }

        public ClientPlayerCharactersContainer PlayerCharacters {
            get {
                return playerCharacters;
            }
        }

        public void SetCharacters(ClientPlayerCharactersContainer playerCharacters) {
            this.playerCharacters = playerCharacters;
            Debug.Log("Characters received");
            Debug.Log(playerCharacters.ToString());
        }

        public static void GetCharacters(string gameRefId, string login) {
            Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                {(byte)ParameterCode.GameRefId, gameRefId },
                {(byte)ParameterCode.Login, login}
            };
            SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.GetCharacters, parameters, true, Settings.ItemChannel);
        }

        public static void CreateCharacter(string gameRefID, Race race, Workshop workshop, string name) {
            Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                {(byte)ParameterCode.GameRefId, gameRefID },
                {(byte)ParameterCode.Race, (byte)race},
                {(byte)ParameterCode.WorkshopId, (byte)workshop},
                {(byte)ParameterCode.DisplayName, name}
            };
            SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.CreateCharacter, parameters, true, Settings.ItemChannel);
        }

        public static void DeleteCharacter(string gameRefID, string charaterID ) {
            Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                {(byte)ParameterCode.GameRefId, gameRefID },
                {(byte)ParameterCode.CharacterId, charaterID}
            };
            SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.DeleteCharacter, parameters, true, Settings.ItemChannel);
        }

        public static void SelectCharacter(string gameRefID, string characterID ) {
            Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                {(byte)ParameterCode.GameRefId, gameRefID },
                {(byte)ParameterCode.CharacterId, characterID }
            };
            SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.SelectCharacter,
                parameters, true, Settings.ItemChannel);
        }

        public void WriteMailMessage(string targetLogin, string title, string body, IInventoryObjectInfo[] inAttachments) {
            //Hashtable items
            Hashtable itemsToRemove = new Hashtable();
            foreach(var attach in inAttachments) {
                itemsToRemove.Add(attach.Id, (byte)attach.Type);
            }
            NRPC.DestroyInventoryItems(InventoryType.ship, itemsToRemove);
            Operations.WriteMailMessage(targetLogin, title, body, inAttachments);
        }

        public void DeleteMailMessage(string messageId) {
            Operations.DeleteMailMessage(messageId);
        }

        public void TakeAttachment(string messageId, string attachmentId) {
            if(NetworkGame.Instance().Inventory.SlotsUsed >= NetworkGame.Instance().Inventory.MaxSlots) {
                Debug.LogError("no free space in inventory");
                return;
            }
            Operations.DeleteAttachment(messageId, attachmentId);
        }

        public void GetMails() { Operations.GetMails(); }

        public static class Operations {
            public static void WriteMailMessage(string targetLogin, string title, string body, IInventoryObjectInfo[] inAttachments) {
                object[] attachments = null;
                if(inAttachments == null || inAttachments.Length == 0 ) {
                    attachments = new object[] { };
                } else {
                    attachments = new object[inAttachments.Length];
                    for(int i = 0; i < inAttachments.Length; i++) {
                        attachments[i] = inAttachments[i].rawHash;
                    }
                }

                Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                    { (byte)ParameterCode.SourceLogin, LoginGame.Instance().login },
                    { (byte)ParameterCode.TargetLogin, targetLogin },
                    { (byte)ParameterCode.Title, title },
                    { (byte)ParameterCode.Body, body },
                    { (byte)ParameterCode.Attachments, attachments }
                };
                SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.WriteMailMessage, parameters, true, Settings.ItemChannel);
            }

            public static void DeleteMailMessage(string messageId) {
                Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                    { (byte)ParameterCode.MessageId, messageId },
                    { (byte)ParameterCode.SourceLogin, LoginGame.Instance().login }
                };
                SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.DeleteMailMessage, parameters, true, Settings.ItemChannel);
            }

            public static void DeleteAttachment(string messageId, string attachmentId ) {
                Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                    { (byte)ParameterCode.Login, LoginGame.Instance().login },
                    { (byte)ParameterCode.MessageId, messageId },
                    { (byte)ParameterCode.AttachmentId, attachmentId }
                };
                SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.DeleteAttachment, parameters, true, Settings.ItemChannel);
            }

            public static void GetMails() {
                Dictionary<byte, object> parameters = new Dictionary<byte, object> { { (byte)ParameterCode.GameRefId, LoginGame.Instance().GameRefId } };
                SelectCharacterGame.Instance().SendOperation((byte)SelectCharacterOperationCode.GetMails, parameters, true, Settings.ItemChannel);
            }
        }
    }
}
namespace Nebula.Mmo.Games {
    using Common;
    using Nebula.Client;
    using Nebula.Mmo.Games.Strategies;
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

        public static void GetCharacters(string gameRefId) {
            Dictionary<byte, object> parameters = new Dictionary<byte, object> {
                {(byte)ParameterCode.GameRefId, gameRefId }
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
    }
}
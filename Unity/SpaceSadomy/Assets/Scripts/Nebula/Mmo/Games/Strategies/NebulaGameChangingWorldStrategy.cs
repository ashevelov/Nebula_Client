namespace Nebula.Mmo.Games.Strategies {
    using UnityEngine;
    using System.Collections;
    using System;
    using ExitGames.Client.Photon;
    using Common;
    using System.Collections.Generic;
    using Nebula.Resources;

    public class NebulaGameChangingWorldStrategy : DefaultStrategy {

        public override GameState State {
            get {
                return GameState.NebulaGameChangingWorld;
            }
        }

        public override void OnOperationReturn(BaseGame game, OperationResponse operationResponse) {
            NetworkGame ngame = game as NetworkGame;

            if(operationResponse.ReturnCode == (short)ReturnCode.Ok) {
                switch((OperationCode)operationResponse.OperationCode) {
                    case OperationCode.CreateWorld:
                        {
                            Debug.Log("world created");
                            var position = new float[] { 0.0f, 0.0f, Settings.START_Z };
                            ngame.Avatar.SetPositions(position, position, null, null, 0);
                            var properties = new Hashtable {
                                {(byte)PS.InterestAreaAttached, ngame.Avatar.InterestAreaAttached },
                                {(byte)PS.ViewDistanceEnter, ngame.Settings.ViewDistanceEnter },
                                {(byte)PS.ViewDistanceExit, ngame.Settings.ViewDistanceExit }
                            };
                            global::Nebula.Operations.EnterWorld(ngame,
                                ngame.Engine.GameData.World.Name,
                                properties,
                                ngame.Avatar.Position,
                                ngame.Avatar.Rotation,
                                ngame.Settings.ViewDistanceEnter,
                                ngame.Settings.ViewDistanceExit,
                                ngame.Engine.LoginGame.GameRefId,
                                ngame.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacterId,
                                (Workshop)ngame.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().HomeWorkshop,
                                (Race)ngame.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().Race,
                                ngame.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().ModelHash(),
                                ngame.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().CharacterName,
                                ngame.Engine.LoginGame.login);
                            return;
                        }
                    case OperationCode.EnterWorld:
                        {
                            var worldData = new WorldData();
                            string name = (string)operationResponse.Parameters[(byte)ParameterCode.WorldName];
                            Vector3 max = ((float[])operationResponse.Parameters[(byte)ParameterCode.BottomRightCorner]).toVector();
                            Vector3 min = ((float[])operationResponse.Parameters[(byte)ParameterCode.TopLeftCorner]).toVector();
                            Vector3 tileDimensions = ((float[])operationResponse.Parameters[(byte)ParameterCode.TileDimensions]).toVector();
                            LevelType levelType = (LevelType)(byte)operationResponse.Parameters[(byte)ParameterCode.LevelType];
                            Hashtable worldContent = operationResponse.Parameters[ParameterCode.WorldContent.toByte()] as Hashtable;
                            if (worldContent != null) {
                                ((NetworkGame)game).ClientWorld.ParseInfo(worldContent);
                                worldContent.Print(1);
                            } else {
                                Debug.Log("WaitingForChangeWorld: world content is null");
                            }

                            worldData.SetWorldParameters(name, min, max, tileDimensions, levelType);

                            ngame.Engine.GameData.SetNewWorld(worldData);
                            ngame.SetStrategy(GameState.NebulaGameWorldEntered);
                            global::Nebula.Operations.SetViewDistance(ngame, ngame.Settings.ViewDistanceEnter, ngame.Settings.ViewDistanceExit);
                            var position = new float[] { 0, 0, Settings.START_Z };
                            ngame.Avatar.SetPositions(position, position, null, null, 0);
                            ngame.SetDisconnectAction(NebulaGameDisconnectAction.None);

                            Debug.Log("ENTERED TO WORLD" + ngame.Engine.GameData.World.Name);

                            LoadScenes.Load(DataResources.Instance.ZoneForId(ngame.Engine.GameData.World.Name).Scene());
                            return;
                        }
                }
            } else {
                switch ((OperationCode)operationResponse.OperationCode) {
                    case OperationCode.EnterWorld:
                        {
                            global::Nebula.Operations.CreateWorld((NetworkGame)game, game.Engine.GameData.World.Name);
                            return;
                        }
                }
            }
            base.OnOperationReturn(game, operationResponse);
        }

        public override void OnPeerStatusChanged(BaseGame game, StatusCode statusCode) {
            switch(statusCode) {
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                    {
                        
                        var ngame = game as NetworkGame;

                        ngame.SetStrategy(GameState.NebulaGameDisconnected);
                        ngame.ClearItemCache();
                        if(ngame.Avatar != null ) {
                            ngame.Avatar.DestroyView();
                            ngame.RemoveAvatar();
                        }

                        break;
                    }
                default:
                    {
                        game.DebugReturn(DebugLevel.ERROR, statusCode.ToString());
                        break;
                    }
            }
        }
    }
}

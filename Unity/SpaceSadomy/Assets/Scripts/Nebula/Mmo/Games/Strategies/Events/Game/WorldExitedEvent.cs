namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using System.Collections;
    using System.Collections.Generic;

    public class WorldExitedEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            var ngame = game as NetworkGame;

            if (GameData.instance.worldTransition.HasNextWorld()) {

                ngame.SetStrategy(GameState.NebulaGameChangingWorld);

                if(ngame.Avatar != null ) {
                    ngame.Avatar.DestroyView();
                }

                ngame.Engine.GameData.SetNewWorld(GameData.instance.worldTransition.NextWorld,
                    ngame.Settings.WorldCornerMin, ngame.Settings.WorldCornerMax, ngame.Settings.TileDimensions, LevelType.Space);

                ngame.ClearItemCache();
                ngame.AddItem(ngame.Avatar);

                var pos = ngame.GetSpawnPosition();
                var position = new float[] { pos.x, pos.y, pos.z };

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

            } else {

                if(ngame.Avatar != null ) {
                    ngame.Avatar.DestroyView();
                }
                ngame.ClearItemCache();
                ngame.AddItem(ngame.Avatar);
                GameData.instance.ship.Clear();
                ngame.SetStrategy(GameState.NebulaGameConnected);

                LoadScenes.Load("select_character");
            }
        }
    }

}
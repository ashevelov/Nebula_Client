namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using System.Collections;
    using System.Collections.Generic;

    public class WorldExitedEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            var ngame = game as NetworkGame;

            if (ngame.WorldTransition.HasNextWorld()) {

                ngame.SetStrategy(GameState.NebulaGameChangingWorld);

                if(ngame.Avatar != null ) {
                    ngame.Avatar.DestroyView();
                }

                ngame.Engine.GameData.SetNewWorld(ngame.WorldTransition.NextWorld,
                    ngame.Settings.WorldCornerMin, ngame.Settings.WorldCornerMax, ngame.Settings.TileDimensions, LevelType.Space);

                ngame.ClearItemCache();
                ngame.AddItem(ngame.Avatar);

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
                    ngame.Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().CharacterName);

            } else {

                if(ngame.Avatar != null ) {
                    ngame.Avatar.DestroyView();
                }
                ngame.ClearItemCache();
                ngame.AddItem(ngame.Avatar);
                ngame.Ship.Clear();
                ngame.SetStrategy(GameState.NebulaGameConnected);

                LoadScenes.Load("select_character");
            }
        }
    }

}
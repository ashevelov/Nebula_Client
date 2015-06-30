using Common;
using ExitGames.Client.Photon;
using Nebula.Mmo.Games;
using Nebula.Resources;
using UnityEngine;

namespace Nebula.Mmo.Games.Strategies.Events.Game {
    public class WorkshopExitedEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEvent((NetworkGame)game, eventData);
        }

        private void HandleEvent(NetworkGame game, EventData eventData) {
            if (HangarShip.Get) {
                HangarShip.Get.Stop();
            }
            game.CreateAvatar(game.Engine.LoginGame.GameRefId);
            var position = Position(eventData);
            game.Avatar.SetPositions(position, position, game.Avatar.Rotation, game.Avatar.Rotation, 0);
            game.SetStrategy(GameState.NebulaGameWorldEntered);
            //Application.LoadLevel(DataResources.Instance.ZoneForId(game.ClientWorld.Id).Scene());
            LoadScenes.Load(DataResources.Instance.ZoneForId(GameData.instance.clientWorld.Id).Scene());
        }

        private float[] Position(EventData eventData) {
            return (float[])eventData.Parameters[ParameterCode.Position.toByte()];
        }
    }
}
using Common;
using Game.Space;
using Nebula.Client.Guilds;
using Nebula.Client.Mail;
using Nebula.Client.Notifications;
using UnityEngine;

namespace Nebula {

    public class GameData  {
        public WorldData World { get; private set; }
        public Chat Chat { get; private set; }
        public CurrentObjectContainer CurrentObjectContainer { get; private set; }
        public MailBox mailBox { get; private set; }
        public CharacterNotifications notifications { get; private set; }
        public Guild guild { get; private set; }

        public GameData() {
            mailBox = new MailBox();
            notifications = new CharacterNotifications();
            guild = new Guild();
        }

        public void SetNewWorld(string worldID, Vector3 cornerMin, Vector3 cornerMax, Vector3 tileDimensions, LevelType levelType) {
            World = new WorldData(worldID, cornerMin, cornerMax, tileDimensions, levelType);
            Chat = new Chat(Settings.MAX_CHAT_MESSAGES_COUNT);
            CurrentObjectContainer = new CurrentObjectContainer();
        }

        public void SetNewWorld(WorldData world) {
            World = world;
        }

        public bool HasWorld {
            get {
                return World != null;
            }
        }
    }
}

using Common;
using Nebula;
using Nebula.Mmo.Games;

namespace Game.Space.UI
{
    public static class DevCommands {

        private static string[] Skip(string[] inputArray, int skipCount) {
            int newArrCount = inputArray.Length - skipCount;        
            if (newArrCount <= 0) {
                return new string[] { };
            } else {
                string[] newArray = new string[newArrCount];
                for (int i = 0; i < newArrCount; i++) {
                    newArray[i] = inputArray[skipCount + i];
                }
                return newArray;
            }
        }

        public static void Execute(string command) {
            string[] tokens = command.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 0) {
                switch (tokens[0].ToLower())
                {
                    case "create":
                        string[] createArgs = Skip(tokens, 1);
                        CreateCommand(createArgs);
                        break;
                    case "destroy":
                        string[] destroyArgs = Skip(tokens, 1);
                        DestroyCommand(destroyArgs);
                        break;
                    case "exit":
                        ExitCommand();
                        break;
                    case "change":
                        string[] changeArgs = Skip(tokens, 1);
                        ChangeCommand(changeArgs);
                        break;
                }
            }
        }

        private static void CreateCommand(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "raider":
                        MmoEngine.Get.NebulaGame.Avatar.CreateRaiderAtMe();
                        break;
                }
            }
        }

        private static void DestroyCommand(string[] args) {
            if (args.Length > 0) {
                switch (args[0]) { 
                    case "raider":
                        MmoEngine.Get.NebulaGame.Avatar.DestroyAnyRaider();
                        break;
                }
            }
        }

        private static void ExitCommand() {
            var game = MmoEngine.Get.NebulaGame;
            if (game.Engine.GameData.HasWorld) {
                if(game.Avatar != null ) {
                    game.Avatar.RequestTarget(string.Empty, (byte)ItemType.Avatar, false);
                }
                Operations.ExitWorld(game);
            }
        }

        private static void ChangeCommand( string[] args) {
            if (args.Length > 0)
            {
                var game = MmoEngine.Get.NebulaGame;
                if (game.Engine.GameData.HasWorld)
                {
                    NetworkGame.ChangeWorld(args[0]);
                }
            }
        }
    }
}
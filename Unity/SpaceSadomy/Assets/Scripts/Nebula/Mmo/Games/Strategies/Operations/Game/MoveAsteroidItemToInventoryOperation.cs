using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Game.Network;
using Common;
using Nebula.UI;
using Nebula.Mmo.Games;
using ServerClientCommon;
using Nebula.Mmo.Items;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class MoveAsteroidItemToInventoryOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }
        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            PrintRPCError(response);
            this.UpdateInventorySourceView(response);

            if (Status(response) == ACTION_RESULT.SUCCESS) {
                Debug.Log("successfully move item to inventory");
                Hashtable returnHash = Return(response) as Hashtable;
                object[] asteroidContent = returnHash.Value<object[]>((int)SPC.Content, new object[] { });
                Debug.LogFormat("asteroid content count: {0}", asteroidContent.Length);

                NetworkGame ngame = game as NetworkGame;
                string asteroidID = returnHash.Value<string>((int)SPC.ContainerId, string.Empty);
                byte asteroidType = returnHash.Value<byte>((int)SPC.ContainerType, (byte)ItemType.Avatar);
                if(string.IsNullOrEmpty(asteroidID)) {
                    Debug.LogError("empty asteroid id");
                    return;
                }
                if(asteroidType != (byte)ItemType.Asteroid) {
                    Debug.LogError("invalid asteroid type");
                    return;
                }

                Item item;

                if(!ngame.TryGetItem(asteroidType, asteroidID, out item)) {
                    Debug.LogError("not found asteroid item");
                    return;
                }

                AsteroidItem aItem = item as AsteroidItem;

                if(aItem == null ) {
                    Debug.LogError("invalid item type");
                    return;
                }
                aItem.SetAsteroidContent(asteroidContent);

            }
            

        }
    }
}

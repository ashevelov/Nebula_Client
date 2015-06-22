using Common;
using ExitGames.Client.Photon;
using Game.Network;
using Game.Space;
using Game.Space.UI;
using Nebula.Client.Inventory;
using System.Collections;
using System.Collections.Generic;
using Nebula.UI;
using ServerClientCommon;
using Nebula.Mmo.Games;
using UnityEngine;
using Nebula.Mmo.Items;

namespace Nebula.Mmo.Games.Strategies.Operations.Game {
    public class RequestContainerOperation : BaseGenericOperation
    {
        public override void Handle(BaseGame game, OperationResponse response) {
            HandleOperation((NetworkGame)game, response);
        }

        private void HandleOperation(NetworkGame game, OperationResponse response)
        {
            List<IInventoryObjectInfo> containerList = new List<IInventoryObjectInfo>();
            string containerId = string.Empty;
            byte containerType = (byte)ItemType.Avatar;

            List<ClientInventoryItem> inventoryItems = new List<ClientInventoryItem>();
            if(Result(response) == null )
            {
                return;
            }

            foreach(DictionaryEntry entry in Result(response))
            {
                if(entry.Value is Hashtable)
                {
                    IInventoryObjectInfo objInfo = InventoryObjectInfoFactory.Get(entry.Value as Hashtable);
                    if( objInfo == null )
                    {
                        continue;
                    }
                    containerList.Add(objInfo);
                    inventoryItems.Add(new ClientInventoryItem(objInfo, 1));
                }
                else if((int)entry.Key == (int)SPC.Target)
                {
                    containerId = (string)entry.Value;
                }
                else if((int)entry.Key == (int)SPC.TargetType)
                {
                    containerType = (byte)entry.Value;
                }
            }

            if (inventoryItems.Count > 0) {
                Debug.LogFormat("received count in container = {0}", inventoryItems.Count);
            }

            if(string.IsNullOrEmpty(containerId))
            {
                return;
            }

            Item item;
            if(!game.TryGetItem(containerType, containerId, out item))
            {
                return;
            }

            if(!(item is IInventoryItemsSource))
            {
                return;
            }

            if (inventoryItems.Count > 0) {
                Debug.Log("Container item found".Color("green"));
            }
            (item as IInventoryItemsSource).SetItems(inventoryItems);
            game.Engine.GameData.CurrentObjectContainer.Reset();
            game.Engine.GameData.CurrentObjectContainer.SetContainer(containerId, containerType, containerList);
        }
    }
}

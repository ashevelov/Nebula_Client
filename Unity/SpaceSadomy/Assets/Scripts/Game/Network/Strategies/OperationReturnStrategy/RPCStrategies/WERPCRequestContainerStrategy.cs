using Common;
using ExitGames.Client.Photon;
using Game.Network;
using Game.Space;
using Game.Space.UI;
using Nebula.Client.Inventory;
using System.Collections;
using System.Collections.Generic;
using Nebula.UI;

namespace Nebula {
    public class WERPCRequestContainerStrategy : RPCOperationReturnStrategy
    {
        public override void Handle(NetworkGame game, OperationResponse response)
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
                else if(entry.Key.ToString() == GenericEventProps.target_id)
                {
                    containerId = (string)entry.Value;
                }
                else if(entry.Key.ToString() == GenericEventProps.target_type)
                {
                    containerType = (byte)entry.Value;
                }
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

            (item as IInventoryItemsSource).SetItems(inventoryItems);
            game.CurrentObjectContainer.Reset();
            game.CurrentObjectContainer.SetContainer(containerId, containerType, containerList);
        }
    }
}

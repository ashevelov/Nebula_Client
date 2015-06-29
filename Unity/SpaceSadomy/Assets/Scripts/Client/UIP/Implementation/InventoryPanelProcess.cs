using UnityEngine;
using System.Collections;
using Nebula.Client.Inventory;
using System.Collections.Generic;
using Nebula.Client.Inventory.Objects;
using Nebula;
using Common;
using Nebula.Resources;
using Client.UIC.Implementation;
using Client.UIC;
using Nebula.Client;

namespace Client.UIP.Implementation
{
    public class InventoryPanelProcess : MonoBehaviour
    {

        private IInventoryPanel uicPanel;

        public InventoryType inventoryPanelType = InventoryType.ship; 

        private List<ClientInventoryItem> currentItems = new List<ClientInventoryItem>();

        void Start()
        {
            StartCoroutine(UpdateInfo());
        }

        IEnumerator UpdateInfo()
        {
            if (CheckCondition())
            {
                Debug.Log("UpdateInfo2");
                UpdateItems();
                if (inventoryPanelType == InventoryType.station)
                {
                    UpdateModules();
                }
            }
            Debug.Log("UpdateInfo1");
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(UpdateInfo());
        }

        private void UpdateItems()
        {
            List<ClientInventoryItem> newItems = null;
            if (inventoryPanelType == InventoryType.station)
            {
                newItems = G.Game.Station.StationInventory.OrderedItems();

                Debug.Log("AddInventoryItem " + newItems.Count);
            }
            else
            {
                newItems = G.Inventory().OrderedItems();
            }

            //first search new and modified items in new item list
            foreach (var item in newItems)
            {
                var foundedItem = this.currentItems.Find(it => it.Object.Id == item.Object.Id);
                if (foundedItem == null)
                {
                    Debug.Log("AddInventoryItem");
                    AddInventoryItem(item);
                }
                else if (foundedItem.Count != item.Count)
                {
                    uicPanel.ModifiedItem(item.Object.Id, item.Count);
                }
            }

            //then search items not presented in new list ( its removed )
            foreach (var cItem in currentItems)
            {
                var foundedItem = newItems.Find(it => it.Object.Id == cItem.Object.Id);
                if (foundedItem == null)
                {
                    uicPanel.RemoveItem(cItem.Object.Id);
                }
            }

            this.currentItems = newItems;
        }

        private List<ClientShipModule> newModules = new List<ClientShipModule>();
        private List<ClientShipModule> currentModules = new List<ClientShipModule>();
        void UpdateModules()
        {

            Dictionary<string, IStationHoldableObject> moduleObjects = new Dictionary<string, IStationHoldableObject>();
            Hashtable moduleObjectsHash = null;
            if (G.Game.Station.Hold.TryGetObjects(StationHoldableObjectType.Module, out moduleObjectsHash))
            {
                foreach (DictionaryEntry entry in moduleObjectsHash)
                {
                    moduleObjects.Add(entry.Key.ToString(), entry.Value as IStationHoldableObject);
                }
            }

            foreach (var moduleObj in moduleObjects)
            {
                newModules.Add(moduleObj.Value as ClientShipModule);
            }

            foreach (var module in newModules)
            {
                var foundedModule = this.currentModules.Find(m => m.Id == module.Id);
                if (foundedModule == null)
                {                
                    Debug.Log("AddModuleItem");
                    AddModuleItem( module );
                }
            }

            foreach (var module in this.currentModules)
            {
                var foundedModule = newModules.Find(m => m.Id == module.Id);
                if (foundedModule == null)
                {
                    uicPanel.RemoveItem(module.id);
                }
            }
            //this.currentModules = this.newModules;
        }

        public bool CheckCondition()
        {
            if (uicPanel == null)
            {
                uicPanel = FindObjectOfType<InventoryPanel>();
            }
            //if (G.Game == null || G.Game.PlayerInfo == null || G.PlayerComponent == null)
             //   return false;
            return true;
        }

        private void AddModuleItem(ClientShipModule clientItem)
        {

            string id = clientItem.id;
            string name = "name hz";
            string type = clientItem.Type.ToString();
            string color = clientItem.color.ToString();
            int count = 1;
            IItemInfo info = new ItemInfo();
            info.Icon = SpriteCache.SpriteModule(clientItem.templateId);
            Dictionary<string, System.Action<string>> actions = new Dictionary<string, System.Action<string>>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            uicPanel.AddItem(new InventoryItem(id, info.Icon, color, name, type, count, 0, info, actions));

        }

        private void AddInventoryItem(ClientInventoryItem clientItem)
        {
            string id = clientItem.Object.Id;
            string name = "name hz";
            string type = clientItem.Object.Type.ToString();
            string color = clientItem.Object.MyColor().ToString();
            int count = clientItem.Count;
            IItemInfo info = new ItemInfo();
            info.Icon = SpriteCache.SpriteForItem(clientItem.Object);
            Dictionary<string, System.Action<string>> actions = new Dictionary<string, System.Action<string>>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();


            

            actions.Add("del", (itemId) =>
                {
                    NRPC.DestroyInventoryItem(inventoryPanelType, clientItem.Object.Type, itemId);
                });


            actions.Add("move", (itemId) =>
            {
                if (inventoryPanelType == InventoryType.station)
                {
                    NRPC.MoveItemFromStationToInventory(clientItem.Object.Type, itemId);
                }
                else
                {
                    NRPC.MoveItemFromInventoryToStation(clientItem.Object.Type, itemId);
                }
            });

            parameters.Add("ID", clientItem.Object.Id);

            if(clientItem.Object is MaterialInventoryObjectInfo)
            {
                MaterialInventoryObjectInfo objectInfo = clientItem.Object as MaterialInventoryObjectInfo;
                parameters.Add("Name", objectInfo.Name);
                name = objectInfo.Name;
                info.Parametrs = parameters;
            }else if (clientItem.Object is WeaponInventoryObjectInfo)
            {
                WeaponInventoryObjectInfo objectInfo = clientItem.Object as WeaponInventoryObjectInfo;
                var weaponTemplate = DataResources.Instance.Weapon(objectInfo.Template);

                parameters.Add("Name", weaponTemplate.Name);
                parameters.Add("Workshop", weaponTemplate.Workshop.ToString());
                parameters.Add("Damage", objectInfo.damage.ToString());
                parameters.Add("Level", objectInfo.Level.ToString());
                parameters.Add("Range", objectInfo.Range.ToString());
                name = weaponTemplate.Name;
                info.Parametrs = parameters;
                info.Description = StringCache.Get(weaponTemplate.Description);

                actions.Add("equip", (itemId) =>
                {
                    NRPC.EquipWeapon(inventoryPanelType, itemId);
                });
            }
            else if (clientItem.Object is SchemeInventoryObjectInfo)
            {
                SchemeInventoryObjectInfo objectInfo = clientItem.Object as SchemeInventoryObjectInfo;

                parameters.Add("Name", "Scheme");
                parameters.Add("Workshop", objectInfo.Workshop.ToString());
                parameters.Add("Level", objectInfo.Level.ToString());

                var module = DataResources.Instance.ModuleData(objectInfo.TargetTemplateId);
                parameters.Add("Module type", StringCache.Get(module.NameId.Remove(2)));

                foreach(var material in objectInfo.CraftMaterials)
                {
                    var materialData = DataResources.Instance.OreData(material.Key);
                    parameters.Add(StringCache.Get(materialData.Name), material.Value.ToString());
                }
                name = "Scheme";
                info.Parametrs = parameters;
                info.Description = StringCache.Get("SCHEME_DESC");
            }
            //else if (clientItem.Object is hol)
            //{
            //    MaterialInventoryObjectInfo objectInfo = clientItem.Object as MaterialInventoryObjectInfo;
            //    Dictionary<string, string> parameters = new Dictionary<string, string>();
            //    parameters.Add("Name", objectInfo.Name);
            //    name = objectInfo.Name;
            //    info.Parametrs = parameters;
            //}
           // else

            uicPanel.AddItem(new InventoryItem(id, info.Icon, color, name, type, count, 0, info, actions));
        }
    }
}

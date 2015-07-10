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
using Nebula.UI;

namespace Client.UIP.Implementation
{
    public class InventoryPanelProcess : MonoBehaviour
    {

        private IInventoryPanel uicPanel;

        public InventoryType inventoryPanelType = InventoryType.ship; 

        private List<ClientInventoryItem> currentItems = new List<ClientInventoryItem>();

        void OnEnable()
        {
            StartCoroutine(UpdateInfo());
        }

        IEnumerator UpdateInfo()
        {
            if (CheckCondition())
            {
                UpdateItems();
                if (inventoryPanelType == InventoryType.station)
                {
                    UpdateModules();
                }
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(UpdateInfo());
        }

        private void UpdateItems()
        {
            List<ClientInventoryItem> newItems = null;
            if (inventoryPanelType == InventoryType.station)
            {
                newItems = GameData.instance.station.StationInventory.OrderedItems();//G.StationInventory.OrderedItems();

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

        private List<ClientShipModule> currentModules = new List<ClientShipModule>();
        void UpdateModules()
        {
            List<ClientShipModule> newModules = new List<ClientShipModule>();
            Dictionary<string, IStationHoldableObject> moduleObjects = new Dictionary<string, IStationHoldableObject>();
            Hashtable moduleObjectsHash = null;
               // newItems = G.Game.//G.StationInventory.OrderedItems();

            if (GameData.instance.station.Hold.TryGetObjects(StationHoldableObjectType.Module, out moduleObjectsHash))
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
            this.currentModules = newModules;
        }

        public bool CheckCondition()
        {
            if (uicPanel == null)
            {
                uicPanel = GetComponent<InventoryPanel>();
            }
            //if (G.Game == null || G.Game.PlayerInfo == null || G.PlayerComponent == null)
             //   return false;
            return true;
        }

        private void AddModuleItem(ClientShipModule clientItem)
        {

            string id = clientItem.id;
            string name = clientItem.type.ToString();// "name hz";
            string type = clientItem.type.ToString();
            string color = clientItem.color.ToString();
            int count = 1;
            IItemInfo info = new ItemInfo();
            string iconPath = "ShipInfoModules/" + clientItem.templateId; //.prefab.Replace("Prefabs/Ships/Modules/","");
            Debug.Log(iconPath);
            info.Icon = SpriteCache.SpriteModule(iconPath);
            Dictionary<string, System.Action<string>> actions = new Dictionary<string, System.Action<string>>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();


            actions.Add("del", (itemId) =>
                {
                    Debug.Log("del item " + itemId);
                    NRPC.DestroyModule(itemId);
                });

            //parameters.Add("ID", clientItem.Id);
            parameters.Add(Nebula.Resources.StringCache.Get("WORKSHOP"), clientItem.workshop.ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("LEVEL"), clientItem.level.ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("HP"), ((int)clientItem.hp).ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("RESIST"), ((int)clientItem.resist).ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("SPEED"), ((int)clientItem.speed).ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("CRIT_CHANCE"), ((int)clientItem.critChance).ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("CRIT_DAMAGE"), ((int)clientItem.critDamage).ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("DAMAGE_BONUS"), ((int)clientItem.damageBonus).ToString());
            parameters.Add(Nebula.Resources.StringCache.Get("DISTANS_BONUS"), ((int)clientItem.distanceBonus).ToString());
            name = clientItem.name;
            info.Parametrs = parameters;

            info.SkillIcon = SpriteCache.SpriteSkill("H" + clientItem.skill.ToString("X8"));
            info.SkillDescription = StringCache.Get("H" + clientItem.skill.ToString("X8"));

            actions.Add("equip", (itemId) =>
            {
                Debug.Log("equip item " + itemId);
                NRPC.EquipModuleFromHoldToShip(itemId);
            });

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
                    NRPC.DestroyInventoryItem(inventoryPanelType, currentItems.Find(m => m.Object.Id == itemId).Object.Type, itemId);
                });


            actions.Add("move", (itemId) =>
            {
                if (inventoryPanelType == InventoryType.station)
                {
                    NRPC.MoveItemFromStationToInventory(currentItems.Find(m => m.Object.Id == itemId).Object.Type, itemId);
                }
                else
                {
                    NRPC.MoveItemFromInventoryToStation(currentItems.Find(m => m.Object.Id == itemId).Object.Type, itemId);
                }
            });

            //parameters.Add("ID", clientItem.Object.Id);

            if(clientItem.Object is MaterialInventoryObjectInfo)
            {
                MaterialInventoryObjectInfo objectInfo = clientItem.Object as MaterialInventoryObjectInfo;
                parameters.Add(Nebula.Resources.StringCache.Get("NAME"), Nebula.Resources.StringCache.Get(objectInfo.Id + "_desc"));
                name = Nebula.Resources.StringCache.Get(objectInfo.Id + "_desc");
                info.Parametrs = parameters;
            }else if (clientItem.Object is WeaponInventoryObjectInfo)
            {
                WeaponInventoryObjectInfo objectInfo = clientItem.Object as WeaponInventoryObjectInfo;
                var weaponTemplate = DataResources.Instance.Weapon(objectInfo.Template);

                parameters.Add(Nebula.Resources.StringCache.Get("NAME"), weaponTemplate.Name);
                parameters.Add(Nebula.Resources.StringCache.Get("WORKSHOP"), weaponTemplate.Workshop.ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("DAMAGE"), objectInfo.damage.ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("LEVEL"), objectInfo.Level.ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("RANGE"), objectInfo.Range.ToString());
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

                parameters.Add(Nebula.Resources.StringCache.Get("NAME"), Nebula.Resources.StringCache.Get("SCHEME"));
                parameters.Add(Nebula.Resources.StringCache.Get("WORKSHOP"), objectInfo.Workshop.ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("LEVEL"), objectInfo.Level.ToString());

                var module = DataResources.Instance.ModuleData(objectInfo.TargetTemplateId);
                parameters.Add(Nebula.Resources.StringCache.Get("MODULE_TYPE"), StringCache.Get(module.NameId.Remove(2)));

                foreach(var material in objectInfo.CraftMaterials)
                {
                    var materialData = DataResources.Instance.OreData(material.Key);
                    parameters.Add(StringCache.Get(materialData.Name), material.Value.ToString());
                }
                name = Nebula.Resources.StringCache.Get("SCHEME");
                info.Parametrs = parameters;
                info.Description = StringCache.Get("SCHEME_DESC");

                if (inventoryPanelType == InventoryType.station)
                {
                    actions.Add("craft", (itemId) =>
                    {
                        CraftModele(itemId);
                        //NRPC.EquipWeapon(inventoryPanelType, itemId);
                    });
                }
            }
            //else if (clientItem.Object is CreditsObjectInfo)
            //{
            //    CreditsObjectInfo objectInfo = clientItem.Object as CreditsObjectInfo;

            //    parameters.Add("Name", "Credits");
            //    parameters.Add("Count", objectInfo.Count().ToString());
            //    name = "Credits";
            //    info.Parametrs = parameters;
            //    info.Description = StringCache.Get("SCHEME_DESC");
            //}
            //else if (clientItem.Object is hol)
            //{
            //    MaterialInventoryObjectInfo objectInfo = clientItem.Object as MaterialInventoryObjectInfo;
            //    Dictionary<string, string> parameters = new Dictionary<string, string>();
            //    parameters.Add("Name", objectInfo.Name);
            //    name = objectInfo.Name;
            //    info.Parametrs = parameters;
            //}
           // else

            uicPanel.AddItem(new InventoryItem(id, info.Icon, color, name, type, count, 0, info, new Dictionary<string, System.Action<string>>(actions)));
            actions = null;
        }

        private void CraftModele(string itemId)
        {
            SchemeInventoryObjectInfo scheme = currentItems.Find(m => m.Object.Id == itemId).Object as SchemeInventoryObjectInfo;
            if (scheme == null)
            {
                return;
            }

            if (PlayerHasMaterials(scheme))
            {
                ActionProgressView.DataObject data = new ActionProgressView.DataObject
                {
                    ActionText = Nebula.Resources.StringCache.Get("CRAFTING"),
                    Duration = 1f,
                    CompleteAction = () =>
                    {
                        //if (MainCanvas.Get.Exists(CanvasPanelType.SchemeCraftView))
                        //{
                            NRPC.TransformInventoryObjectAndMoveToStationHold(scheme.Type, scheme.Id);
                            Debug.Log("Crafting");
                        //}
                    }
                };

                MainCanvas.Get.Show(CanvasPanelType.ActionProgressView, data);
            }
            else
            {
                G.Game.Engine.GameData.Chat.PastLocalMessage("You don't has required materials for crafting modules...");
            }
        }

        private bool PlayerHasMaterials(SchemeInventoryObjectInfo scheme)
        {
            bool hasAll = true;
            foreach (var pMaterial in scheme.CraftMaterials)
            {
                if (GameData.instance.station.StationInventory.ItemCount(InventoryObjectType.Material, pMaterial.Key) < pMaterial.Value)
                {
                    hasAll = false;
                    break;
                }
            }
            return hasAll;
        }
    }


}

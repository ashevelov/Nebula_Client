using UnityEngine;
using System.Collections;
using Client.UIC;
using Client.UIC.Implementation;
using Nebula;
using Nebula.Client;
using Nebula.Resources;
using System.Collections.Generic;
using Nebula.Client.Inventory;
using Nebula.Client.Inventory.Objects;
using Nebula.Client.Auction;
using Common;


namespace Client.UIP.Implementation
{
    public class StoreProcess : MonoBehaviour
    {
        IStorePanel uicPanel;
        void OnEnable()
        {

            if (CheckCondition())
            {
                UpdateItems();
            }

            uicPanel.AddFilterHendler += AddFilter;
        }
        void OnDisable()
        {
            foreach (var filterToggle in GetComponentsInChildren<FilterToggle>())
            {
                filterToggle.AddFilterHendler -= AddFilter;
                filterToggle.RemoveFilterHendler -= RemoveFilter;
            }
        }

        private void AddFilter(Nebula.Client.Auction.AuctionFilter filter)
        {
            GameData.instance.auction.SetFilter(filter);
        }

        private void RemoveFilter(Nebula.Client.Auction.AuctionFilter filter)
        {
            GameData.instance.auction.RemoveFilter(filter);
        }
        public bool CheckCondition()
        {
            if (uicPanel == null)
            {
                uicPanel = GetComponent<StorePanel>();
            }
            return true;
        }
        //-----------------TEMP ITEMS INVENTORY-------------------
        public void UpdateItems()
        {
            Debug.Log("MinPrice=" + uicPanel.GetMinPrice());
            Debug.Log("MaxPrice=" + uicPanel.GetMaxPrice());
            Debug.Log("Search Name=" + uicPanel.GetSearchName());


            CheckCondition();
            List<AuctionItem> aucItems = GameData.instance.auction.items;

            List<IInventoryItem> items = new List<IInventoryItem>();
            //first search new and modified items in new item list
            foreach (var auitem in aucItems)
            {
                foreach (var oi in auitem.objectInfo.Keys)
                {
                    Debug.Log(oi);
                }
             //   InventoryItem item = auitem.
                //items.Add(AddInventoryItem(item));
            }
            uicPanel.UpdateItems(items);
        }
        //-----------------/TEMP ITEMS INVENTORY______________________

        private void BuyItem(string id)
        {
            ConfirmationDialog.Setup("Buy " + id + " ?", () =>
                {
                Debug.Log("Buy " + id);
                });
        }

        private void EditPrice(string id)
        {
            Sprite icon;
            ClientInventoryItem clientItem = GameData.instance.inventory.GetItem(id);
            icon = SpriteCache.SpriteForItem(clientItem.Object);
            EditPricePanelProcess.Init(id, icon, 20);
            Debug.Log("EditPrice " + id);
        }


        private InventoryItem AddModuleItem(ClientShipModule clientItem)
        {

            string id = clientItem.id;
            string name = Nebula.Resources.StringCache.Get("type_module");// "name hz";
            string type = Nebula.Resources.StringCache.Get("type_module");
            string color = clientItem.color.ToString();
            int count = 1;
            IItemInfo info = new ItemInfo();
            string iconPath = "ShipInfoModules/" + clientItem.templateId; //.prefab.Replace("Prefabs/Ships/Modules/","");
            Debug.Log(iconPath);
            info.Icon = SpriteCache.SpriteModule(iconPath);
            Dictionary<string, System.Action<string>> actions = new Dictionary<string, System.Action<string>>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();


            actions.Add("buy", (itemId) =>
            {
                BuyItem(itemId);
            });

            actions.Add("edit", (itemId) =>
            {
                EditPrice(itemId);
            });

            //parameters.Add("ID", clientItem.Id);
            


            return new InventoryItem(id, info.Icon, color, name, type, count, 0, info, actions);

        }

        private InventoryItem AddInventoryItem(AuctionItem auctItem)
        {

            ClientInventoryItem clientItem = null;
            string namea = auctItem.objectInfo.selectName();
            string id = clientItem.Object.Id;
            string name = Nebula.Resources.StringCache.Get("type_" + clientItem.Object.Type.ToString().ToLower());
            string type = Nebula.Resources.StringCache.Get("type_" + clientItem.Object.Type.ToString().ToLower());
            string color = clientItem.Object.MyColor().ToString();
            int count = clientItem.Count;
            IItemInfo info = new ItemInfo();
            info.Icon = SpriteCache.SpriteForItem(clientItem.Object);
            Dictionary<string, System.Action<string>> actions = new Dictionary<string, System.Action<string>>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();


            actions.Add("buy", (itemId) =>
            {
                BuyItem(itemId);
            });

            actions.Add("edit", (itemId) =>
            {
                EditPrice(itemId);
            });


            //parameters.Add("ID", clientItem.Object.Id);
            if (clientItem.Object is ClientShipModule)
            {

                ClientShipModule clientModuleItem = clientItem.Object as ClientShipModule;

                parameters.Add(Nebula.Resources.StringCache.Get("WORKSHOP"), Nebula.Resources.StringCache.Get("WORKSHOP_" + clientModuleItem.workshop.ToString().ToUpper()));
                parameters.Add(Nebula.Resources.StringCache.Get("inventory_level"), clientModuleItem.level.ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("hum_inventoryHP"), ((int)clientModuleItem.hp).ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("hum_inventoryresistance"), ((int)clientModuleItem.resist).ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("inventoryspeed"), ((int)clientModuleItem.speed).ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("inventorycritchance"), ((int)clientModuleItem.critChance).ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("inventorycritdamage"), ((int)clientModuleItem.critDamage).ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("inventorydamagebonus"), ((int)clientModuleItem.damageBonus).ToString());
                name = Nebula.Resources.StringCache.Get(clientModuleItem.type + "_NAME");//info.type+"_NAME")
                info.Parametrs = parameters;

                info.SkillIcon = SpriteCache.SpriteSkill("H" + clientModuleItem.skill.ToString("X8"));
                info.SkillDescription = StringCache.Get("H" + clientModuleItem.skill.ToString("X8"));
            }
            else if (clientItem.Object is MaterialInventoryObjectInfo)
            {
                MaterialInventoryObjectInfo objectInfo = clientItem.Object as MaterialInventoryObjectInfo;
                parameters.Add(Nebula.Resources.StringCache.Get("inventory_name"), Nebula.Resources.StringCache.Get(objectInfo.Id + "_desc"));
                name = Nebula.Resources.StringCache.Get(objectInfo.Id + "_desc");
                info.Parametrs = parameters;
            }
            else if (clientItem.Object is WeaponInventoryObjectInfo)
            {
                WeaponInventoryObjectInfo objectInfo = clientItem.Object as WeaponInventoryObjectInfo;
                var weaponTemplate = DataResources.Instance.Weapon(objectInfo.Template);

                parameters.Add(Nebula.Resources.StringCache.Get("inventory_name"), Nebula.Resources.StringCache.Get("WEAPONNAME")); // weaponTemplate.Name);
                parameters.Add(Nebula.Resources.StringCache.Get("WORKSHOP"), Nebula.Resources.StringCache.Get("WORKSHOP_" + weaponTemplate.Workshop.ToString().ToUpper()));
                parameters.Add(Nebula.Resources.StringCache.Get("inventorydamage"), objectInfo.damage.ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("inventory_level"), objectInfo.Level.ToString());
                parameters.Add(Nebula.Resources.StringCache.Get("inventoryoptimal"), objectInfo.OptimalDistance.ToString());
                name = Nebula.Resources.StringCache.Get("WEAPONNAME");
                info.Parametrs = parameters;
                info.Description = StringCache.Get(weaponTemplate.Description);
            }
            else if (clientItem.Object is SchemeInventoryObjectInfo)
            {
                SchemeInventoryObjectInfo objectInfo = clientItem.Object as SchemeInventoryObjectInfo;

                parameters.Add(Nebula.Resources.StringCache.Get("inventory_name"), Nebula.Resources.StringCache.Get("type_" + clientItem.Object.Type.ToString().ToLower()));
                parameters.Add(Nebula.Resources.StringCache.Get("WORKSHOP"), Nebula.Resources.StringCache.Get("WORKSHOP_" + objectInfo.Workshop.ToString().ToUpper()));
                parameters.Add(Nebula.Resources.StringCache.Get("inventory_level"), objectInfo.Level.ToString());

                var module = DataResources.Instance.ModuleData(objectInfo.TargetTemplateId);
                parameters.Add(Nebula.Resources.StringCache.Get("inventory_type"), StringCache.Get(module.NameId.Remove(2)));

                foreach (var material in objectInfo.CraftMaterials)
                {
                    var materialData = DataResources.Instance.OreData(material.Key);
                    parameters.Add(Nebula.Resources.StringCache.Get(materialData.Id + "_desc"), material.Value.ToString());
                }
                name = Nebula.Resources.StringCache.Get("type_" + clientItem.Object.Type.ToString().ToLower());
                info.Parametrs = parameters;
                info.Description = StringCache.Get("SCHEME_DESC");
            }

            return new InventoryItem(id, info.Icon, color, name, type, count, 0, info, new Dictionary<string, System.Action<string>>(actions));
        }
    }

}

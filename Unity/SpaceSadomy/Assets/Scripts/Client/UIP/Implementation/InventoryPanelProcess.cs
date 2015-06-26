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

namespace Client.UIP.Implementation
{
    public class InventoryPanelProcess : MonoBehaviour
    {

        private IInventoryPanel uicPanel;

        private List<ClientInventoryItem> currentItems = new List<ClientInventoryItem>();

        void Start()
        {
            StartCoroutine(UpdateInfo());
        }

        IEnumerator UpdateInfo()
        {
            if (CheckCondition())
            {
                List<ClientInventoryItem> newItems = null;
                newItems = G.Inventory().OrderedItems();

                //first search new and modified items in new item list
                foreach (var item in newItems)
                {
                    var foundedItem = this.currentItems.Find(it => it.Object.Id == item.Object.Id);
                    if (foundedItem == null)
                    {
                        AddItem(item);
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

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(UpdateInfo());
        }

        public bool CheckCondition()
        {
            if (uicPanel == null)
            {
                uicPanel = FindObjectOfType<InventoryPanel>();
            }
            if (G.Game == null || G.Game.PlayerInfo == null || G.PlayerComponent == null)
                return false;
            return true;
        }

        private void AddItem(ClientInventoryItem clientItem)
        {
            string id = clientItem.Object.Id;
            string name = "name hz";
            string type = clientItem.Object.Type.ToString();
            string color = clientItem.Object.MyColor().ToString();
            int count = clientItem.Count;
            IItemInfo info = new ItemInfo();
            info.Icon = SpriteCache.SpriteForItem(clientItem.Object);
            Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();

            actions.Add("del", () =>
                {
                    NRPC.DestroyInventoryItem(InventoryType.ship, clientItem.Object.Type, clientItem.Object.Id);
                });

            if(clientItem.Object is MaterialInventoryObjectInfo)
            {
                MaterialInventoryObjectInfo objectInfo = clientItem.Object as MaterialInventoryObjectInfo;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Name", objectInfo.Name);
                name = objectInfo.Name;
                info.Parametrs = parameters;
            }else if (clientItem.Object is WeaponInventoryObjectInfo)
            {
                WeaponInventoryObjectInfo objectInfo = clientItem.Object as WeaponInventoryObjectInfo;
                var weaponTemplate = DataResources.Instance.Weapon(objectInfo.Template);

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Name", weaponTemplate.Name);
                parameters.Add("Workshop", weaponTemplate.Workshop.ToString());
                parameters.Add("Damage", objectInfo.damage.ToString());
                parameters.Add("Level", objectInfo.Level.ToString());
                parameters.Add("Range", objectInfo.Range.ToString());
                name = weaponTemplate.Name;
                info.Parametrs = parameters;
                info.Description = StringCache.Get(weaponTemplate.Description);

                actions.Add("equip", () =>
                {
                    NRPC.EquipWeapon(InventoryType.ship, clientItem.Object.Id);
                });
            }
            else if (clientItem.Object is SchemeInventoryObjectInfo)
            {
                SchemeInventoryObjectInfo objectInfo = clientItem.Object as SchemeInventoryObjectInfo;

                Dictionary<string, string> parameters = new Dictionary<string, string>();
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

            uicPanel.AddItem(new InventoryItem(id, info.Icon, color, name, type, count, 0, info, actions));
        }
    }
}

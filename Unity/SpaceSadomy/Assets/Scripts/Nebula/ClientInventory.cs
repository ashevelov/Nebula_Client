namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Common;
    using Nebula.Client.Inventory;
    using ServerClientCommon;

    public class ClientInventory : Dictionary<InventoryObjectType, Dictionary<string, ClientInventoryItem>> {

        private int maxSlots;

        public ClientInventory() {
            maxSlots = 0;
        }

        public ClientInventory(int maxSlots) {
            this.maxSlots = maxSlots;
        }

        public ClientInventory(Hashtable info) {
            ParseInfo(info);
        }

        public int MaxSlots {
            get {
                return maxSlots;
            }
        }

        public Dictionary<InventoryObjectType, Dictionary<string, ClientInventoryItem>> Items {
            get {
                return this;
            }
        }

        public int SlotsUsed {
            get {
                int count = 0;
                foreach(var kv in this) {
                    count += kv.Value.Count;
                }
                return count;
            }
        }

        public bool TryGetItem(InventoryObjectType type, string id, out ClientInventoryItem item) {
            item = null;
            if (ContainsKey(type)) {
                Dictionary<string, ClientInventoryItem> typedItems = this[type];
                if(typedItems.ContainsKey(id)) {
                    item = typedItems[id];
                    return true;
                }
            }
            return false;
        }

        public bool AddItem(IInventoryObjectInfo obj, int count) {
            ClientInventoryItem item = null;
            if(TryGetItem(obj.Type, obj.Id, out item)) {
                item.Add(count);
                return true;
            } else {
                if(SlotsUsed < MaxSlots ) {
                    if(ContainsKey(obj.Type)) {
                        ClientInventoryItem nItem = new ClientInventoryItem();
                        nItem.Set(obj, count);
                        this[obj.Type].Add(obj.Id, nItem);
                    } else {
                        ClientInventoryItem nItem = new ClientInventoryItem();
                        nItem.Set(obj, count);
                        this.Add(obj.Type, new Dictionary<string, ClientInventoryItem> { { obj.Id, nItem } });
                    }
                    return true;
                }
            }
            return false;
        }

        public void RemoveItem(InventoryObjectType type, string id, int count) {
            ClientInventoryItem item = null;
            if(TryGetItem(type, id, out item)) {
                item.Remove(count);
                if(!item.Has) {
                    this[type].Remove(item.Object.Id);
                }
            }
        }

        public int ItemCount(InventoryObjectType type, string id) {
            ClientInventoryItem item = null;
            if(TryGetItem(type, id, out item)) {
                return item.Count;
            }
            return 0;
        }

        public bool HasItem(InventoryObjectType type, string id) {
            ClientInventoryItem item = null;
            if(TryGetItem(type, id, out item)) {
                return item.Has;
            }
            return false;
        }

        public override string ToString() {
            return string.Format("inventory: {0}/{1}", SlotsUsed, MaxSlots);
        }

        public int ChangeMaxSlots(int newValue) {
            int oldValue = maxSlots;
            maxSlots = newValue;
            return oldValue;
        }

        public void Replace(ClientInventory newInventory) {
            Clear();
            ChangeMaxSlots(newInventory.MaxSlots);
            foreach(var pair in newInventory) {
                foreach(var pair2 in pair.Value) {
                    if(!ContainsKey(pair.Key)) {
                        Add(pair.Key, new Dictionary<string, ClientInventoryItem>());
                    }
                    this[pair.Key].Add(pair2.Key, pair2.Value);
                }
            }
        }

        public void ReplaceItem(ClientInventoryItem item) {
            if (!ContainsKey(item.Object.Type)) {
                Add(item.Object.Type, new Dictionary<string, ClientInventoryItem>());
            }

            var typedItems = this[item.Object.Type];
            if(typedItems.ContainsKey(item.Object.Id)) {
                typedItems[item.Object.Id].Set(item.Object, item.Count);
            } else {
                typedItems.Add(item.Object.Id, item);
            }
        }

        public List<ClientInventoryItem> OrderedItems() {
            List<InventoryObjectType> objectTypes = new List<InventoryObjectType>();
            foreach(var entry in this) {
                objectTypes.Add(entry.Key);
            }
            objectTypes.Sort();

            List<ClientInventoryItem> result = new List<ClientInventoryItem>();
            foreach(InventoryObjectType type in objectTypes) {
                var typedItems = this[type];
                List<string> ids = new List<string>();
                foreach(var e in typedItems) {
                    ids.Add(e.Key);
                }
                ids.Sort();
                foreach(string id in ids) {
                    result.Add(typedItems[id]);
                }
            }
            return result;
        }

        public bool CheckMaterial(string id, int count) {
            ClientInventoryItem item;
            if(TryGetItem(InventoryObjectType.Material, id, out item)) {
                if(item.Count >= count) {
                    return true;
                }
            }
            return false;
        }

        public void ParseInfo(Hashtable info) {
            maxSlots = info.Value<int>((int)SPC.MaxSlots, 0);
            
            object[] receivedItems = info.Value<object[]>((int)SPC.Items, new object[] { });

            foreach(var obj in receivedItems) {
                if(!(obj is Hashtable)) {
                    throw new System.Exception("object must be hash");
                }
                Hashtable itemTable = obj as Hashtable;
                int itemCount = itemTable.Value<int>((int)SPC.Count, 0);
                InventoryObjectType itemType = (InventoryObjectType)(byte)(int)itemTable.Value<int>((int)SPC.ItemType, (byte)InventoryObjectType.Weapon);

                switch (itemType) {
                    case InventoryObjectType.Weapon:
                        {
                            AddItem(InventoryObjectInfoFactory.GetClientWeaponObject(itemTable), itemCount);
                            break;
                        }
                    case InventoryObjectType.Scheme:
                        {
                            AddItem(InventoryObjectInfoFactory.GetClientSchemeObject(itemTable), itemCount);
                            break;
                        }
                    case InventoryObjectType.Material:
                        {
                            AddItem(InventoryObjectInfoFactory.GetClientMaterialObject(itemTable), itemCount);
                            break;
                        }
                    case InventoryObjectType.DrillScheme:
                        {
                            AddItem(InventoryObjectInfoFactory.GetClientDrillSchemeObject(itemTable), itemCount);
                            break;
                        }
                    case InventoryObjectType.credits:
                        {
                            AddItem(InventoryObjectInfoFactory.GetCreditsObject(itemTable), itemCount);
                            break;
                        }
                }
            }
        }

    }
}

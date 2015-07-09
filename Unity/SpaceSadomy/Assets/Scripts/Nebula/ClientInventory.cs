namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Common;
    using Nebula.Client.Inventory;
    using ServerClientCommon;



    public class ClientInventory  {


        private List<Entry> mEntries = new List<Entry>();
        private int maxSlots;

        public class Entry {
            public InventoryObjectType objectType { get; set; }
            public string objectID { get; set; }
            public ClientInventoryItem item { get; set; }
        }

        

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
                Dictionary<InventoryObjectType, Dictionary<string, ClientInventoryItem>> result = new Dictionary<InventoryObjectType, Dictionary<string, ClientInventoryItem>>();
                foreach(var entry in mEntries) {
                    if(result.ContainsKey(entry.objectType)) {
                        result[entry.objectType].Add(entry.objectID, entry.item);
                    } else {
                        result.Add(entry.objectType, new Dictionary<string, ClientInventoryItem> { { entry.objectID, entry.item } });
                    }
                }
                return result;
            }
        }

        public int SlotsUsed {
            get {
                return mEntries.Count;
            }
        }

        public bool TryGetItem(InventoryObjectType type, string id, out ClientInventoryItem item) {
            item = null;
            foreach(var  entry in mEntries ) {
                if(entry.objectType == type && entry.objectID == id ) {
                    item = entry.item;
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
                    ClientInventoryItem nItem = new ClientInventoryItem();
                    nItem.Set(obj, count);
                    mEntries.Add(new Entry { objectType = obj.Type, objectID = obj.Id, item = nItem });
                    return true;
                }
            }
            return false;
        }

        private void Remove(InventoryObjectType type, string id) {
            int index = -1;
            for(int i = 0; i < mEntries.Count; i++) {
                if(type == mEntries[i].objectType && id == mEntries[i].objectID) {
                    index = i;
                    break;
                }
            }

            if(index >= 0 ) { mEntries.RemoveAt(index); }
        }

        public void RemoveItem(InventoryObjectType type, string id, int count) {
            ClientInventoryItem item = null;
            if(TryGetItem(type, id, out item)) {
                item.Remove(count);
                if(!item.Has) {
                    Remove(type, item.Object.Id);
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

        public void Clear() {
            mEntries.Clear();
        }

        public void Replace(ClientInventory newInventory) {
            Clear();
            ChangeMaxSlots(newInventory.MaxSlots);
            foreach(var pair in newInventory.Items) {
                foreach(var pair2 in pair.Value) {
                    AddItem(pair2.Value.Object, pair2.Value.Count);
                }
            }
        }

        public void ReplaceItem(ClientInventoryItem item) {

            ClientInventoryItem existingItem;
            if(TryGetItem(item.Object.Type, item.Object.Id, out existingItem)) {
                existingItem.Set(item.Object, item.Count);
            } else {
                AddItem(item.Object, item.Count);
            }

        }

        private List<ClientInventoryItem> GetTypedItems(InventoryObjectType type) {
            List<ClientInventoryItem> result = new List<ClientInventoryItem>();
            foreach(var entry in mEntries) {
                if (entry.objectType == type) {
                    result.Add(entry.item);
                }
            }
            return result;
        }

        public Dictionary<string, ClientInventoryItem> GetTypedDict(InventoryObjectType type) {
            Dictionary<string, ClientInventoryItem> typedDict = new Dictionary<string, ClientInventoryItem>();
            foreach(var entry in mEntries) {
                if(entry.objectType == type ) {
                    typedDict.Add(entry.objectID, entry.item);
                }
            }
            return typedDict;
        }

        public List<ClientInventoryItem> OrderedItems() {
            List<InventoryObjectType> objectTypes = new List<InventoryObjectType>();
            foreach(var entry in mEntries) {
                if (!objectTypes.Contains(entry.objectType)) { objectTypes.Add(entry.objectType); }
            }
            objectTypes.Sort();

            List<ClientInventoryItem> result = new List<ClientInventoryItem>();

            foreach(InventoryObjectType type in objectTypes) {
                result.AddRange(GetTypedItems(type));
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
                    //case InventoryObjectType.credits:
                    //    {
                    //        AddItem(InventoryObjectInfoFactory.GetCreditsObject(itemTable), itemCount);
                    //        break;
                    //    }
                }
            }
        }

    }
}

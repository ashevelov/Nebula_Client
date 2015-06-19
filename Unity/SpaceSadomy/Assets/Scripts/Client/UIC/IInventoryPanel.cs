using System;
using UIP;

namespace UIC
{
    interface IInventoryPanel
    {
        void AddItem(IInventoryItem item);
        void ModifiedItem(string id, int count);
        void RemoveItem(string id);
        void SetEquipAction(Action<string> action);
    }
}

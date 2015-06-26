using Client.UIP;
using System;

namespace Client.UIC
{
    interface IInventoryPanel
    {
        void AddItem(IInventoryItem item);
        void ModifiedItem(string id, int count);
        void RemoveItem(string id);
    }
}

namespace Nebula {
    using Common;
    using Nebula.Client.Inventory;
    using System.Collections.Generic;

    public interface IInventoryItemsSource {
        List<ClientInventoryItem> Items { get; }
        string Id { get; }
        void SetItems(List<ClientInventoryItem> items);
        ItemType SourceType { get;  }
        bool RemoveItem(string itemId);
    }
}

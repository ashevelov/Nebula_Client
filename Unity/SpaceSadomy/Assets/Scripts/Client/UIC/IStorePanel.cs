using Client.UIP;
using Nebula.Client.Auction;
using System.Collections.Generic;

namespace Client.UIC
{
    interface IStorePanel
    {
        int GetMinPrice();
        int GetMaxPrice();
        string GetSearchName();
        event System.Action<AuctionFilter> AddFilterHendler;
        event System.Action<AuctionFilter> RemoveFilterHendler;
        event System.Action UpdateItemsHendler;
        int GetCurrentPage();
        void SetPageCount(int i);
        void UpdateItems(List<IInventoryItem> items);
    }
}

using UnityEngine;
using System.Collections;

namespace Client.UIP.Implementation
{
    public class TypeFilter : FilterToggle
    {
        public Common.AuctionObjectType objectType;
        public override void UpdateFilter()
        {
            base.UpdateFilter();
            if (toggle.isOn)
            {
                AddFilter(new Nebula.Client.Auction.ObjectTypeFilter { objectType = this.objectType });
            }
            else
            {
                RemoveFilter(new Nebula.Client.Auction.ObjectTypeFilter { objectType = this.objectType });
            }
        }
    }
}

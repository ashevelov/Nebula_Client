using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UIP.Implementation
{
    public class WorkshopFilter : FilterToggle
    {
        public Common.Workshop workshop;
        public override void UpdateFilter()
        {
            base.UpdateFilter();
            if (toggle.isOn)
            {
                AddFilter(new Nebula.Client.Auction.WorkshopFilter { workshop = this.workshop });
            }
            else
            {
                RemoveFilter(new Nebula.Client.Auction.WorkshopFilter { workshop = this.workshop });
            }
        }
    }
}
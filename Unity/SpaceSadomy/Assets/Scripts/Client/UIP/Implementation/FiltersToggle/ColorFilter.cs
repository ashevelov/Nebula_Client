using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UIP.Implementation
{
    public class ColorFilter : FilterToggle
    {
        public Common.ObjectColor color;
        public override void UpdateFilter()
        {
            base.UpdateFilter();
            if (toggle.isOn)
            {
                AddFilter(new Nebula.Client.Auction.ColorFilter { color = this.color });
            }
            else
            {
                RemoveFilter(new Nebula.Client.Auction.ColorFilter { color = this.color });
            }
        }
    }
}

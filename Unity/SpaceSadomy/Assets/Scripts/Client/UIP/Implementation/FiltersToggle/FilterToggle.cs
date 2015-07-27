using UnityEngine;
using System.Collections;
using Nebula.Client.Auction;
using UnityEngine.UI;

namespace Client.UIP.Implementation
{
    public abstract class FilterToggle : MonoBehaviour
    {
        private Toggle _toggle;
        protected Toggle toggle
        {
            get
            {
                if (_toggle == null)
                    _toggle = GetComponent<Toggle>();
                return _toggle;
            }
        }
        public virtual void UpdateFilter() { }

        public event System.Action<AuctionFilter> AddFilterHendler;
        public event System.Action<AuctionFilter> RemoveFilterHendler;
        protected void AddFilter(AuctionFilter filter)
        {
            AddFilterHendler(filter);
        }
        protected void RemoveFilter(AuctionFilter filter)
        {
            RemoveFilterHendler(filter);
        }
    }
}

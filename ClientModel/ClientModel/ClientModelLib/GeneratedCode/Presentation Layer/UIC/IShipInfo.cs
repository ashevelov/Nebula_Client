using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIC
{
    public interface IShipInfo : IView
    {
        IShipProperties ShipProperties
        {
            get;
        }
    }
}


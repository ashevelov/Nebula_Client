using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIC
{
    public interface IShipProperties : IViewProperties
    {
        IObjectInfo CB { get; set; }
        IObjectInfo DM { get; set; }
        IObjectInfo DF { get; set; }
        IObjectInfo CM { get; set; }
        IObjectInfo ES { get; set; }
        IObjectInfo Ship { get; set; }
        IObjectInfo Weapon { get; set; }
    }
}


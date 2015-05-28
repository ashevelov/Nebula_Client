using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIC.CargoComponents
{
    public interface ICargoProperties : IViewProperties
    {
        IValue<int> MaxSlots
        {
            get;
            set;
        }
        IValue<int> CurentSlots
        {
            get;
            set;
        }

        IEnumerable<ICargoItem> Items
        {
            get;
            set;
        }

    }
}


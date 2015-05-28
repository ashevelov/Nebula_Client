using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIC.CargoComponents
{
    public interface ICargoItem
    {
        string ID { get; set; }

        string Name { get; set; }

        string Type { get; set; }

        IValue<int> Count { get; set; }

        List<IEventObject> Events { get; set; }

        IObjectInfo Info { get; set; }
    }
}


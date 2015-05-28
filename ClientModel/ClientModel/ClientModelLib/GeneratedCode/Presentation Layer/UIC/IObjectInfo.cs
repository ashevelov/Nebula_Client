using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIC
{
    public interface IObjectInfo
    {
        string ID { get; set; }
        string Icon { get; set; }

        Dictionary<string, IValue<string>> properties { get; set; }
    }
}

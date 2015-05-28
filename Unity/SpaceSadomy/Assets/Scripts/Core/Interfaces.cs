using UnityEngine;
using System.Collections;

namespace Nebula {

    public interface IServerPropertyParser {
        void ParseProp(string propName, object value);
        void ParseProps(Hashtable properties);
    }
}

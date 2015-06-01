using UnityEngine;
using System.Collections;

namespace Nebula {

    public interface IServerPropertyParser {
        void ParseProp(byte propName, object value);

    }
}

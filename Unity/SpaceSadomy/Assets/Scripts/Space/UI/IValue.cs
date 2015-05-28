using UnityEngine;
using System.Collections;

namespace Game.Space.UI
{
    public interface IValue<T> 
    {
        T Value { get; }

        void SetValue(T val);
    }
}


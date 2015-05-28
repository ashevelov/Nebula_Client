using UnityEngine;
using System.Collections;

namespace UIC.CargoComponents
{
    public interface ICargo : IView
    {
        ICargoProperties CargoProperties
        {
            get;
        }
    }
}


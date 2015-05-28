using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIC.CargoComponents
{
    public abstract class Cargo : MonoBehaviour, ICargo {

        public virtual void Init(IViewProperties properties)
        {
            CargoProperties = properties as ICargoProperties;
        }

        public virtual ICargoProperties CargoProperties
        {
            get;
            private set;
        }

    }
}


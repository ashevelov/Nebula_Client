using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIC.CargoComponents.UIComponents
{
    public class CargoPanel : Cargo
    {

        public CargoPanelInfo cargoPanelInfo;
        public CargoPanelScroll cargoPanelScroll;

        public override void Init(IViewProperties properties)
        {
            base.Init(properties);
        }

    }
}

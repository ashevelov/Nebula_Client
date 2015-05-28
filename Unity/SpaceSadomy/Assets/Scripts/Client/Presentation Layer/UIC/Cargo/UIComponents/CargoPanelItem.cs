using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIC.CargoComponents.UIComponents
{
    public class CargoPanelItem : MonoBehaviour {

        public Text name;
        public Text type;
        public Text count;

        ICargoItem item;
        public IValue<bool> isTarget;

        public void Init(ICargoItem item)
        {
            this.item = item;
            name.text = item.Name;
            type.text = item.Type;
            item.Count.Attach(UpdateCount);
            UpdateCount(item.Count.Val);
        }

        void UpdateCount(int count)
        {
            this.count.text = count.ToString();
        }
    }
}


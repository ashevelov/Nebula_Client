namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;

    public abstract class BaseItemView : MonoBehaviour {

        public abstract void SetObject(ItemInfoView.ItemContentData contentData);
    }
}

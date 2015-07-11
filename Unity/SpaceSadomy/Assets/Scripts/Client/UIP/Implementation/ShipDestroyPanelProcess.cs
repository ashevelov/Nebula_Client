using UnityEngine;
using System.Collections;

public class ShipDestroyPanelProcess : MonoBehaviour {

    public void OkButton()
    {
        MmoEngine.Get.ExitMenu();
        Destroy(this.gameObject);
    }
}

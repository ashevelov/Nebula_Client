using UnityEngine;
using Nebula;


public class DropContainer : MonoBehaviour {

    private Item _item;

    public void Initialize(Item item) {
        _item = item;
    }

    void Update() {

        if (_item != null) {
            var game = MmoEngine.Get.Game;
            transform.position = _item.Position.toVector();
            transform.rotation = Quaternion.Euler(_item.Rotation.toVector());

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "container" && this.name == hit.transform.name)
                    {

                        var player = MmoEngine.Get.Game.Avatar;

                        if (!game.Ship.Destroyed && !player.ShipDestroyed)
                        {
        //                    UIEntity containerUI = UIManager.Get.GetLayout("container");
        //                    if (containerUI._visible == false)
        //                    {
        //                        MmoEngine.Get.Game.CurrentObjectContainer.Reset();
								////G.UI.ContainerView.Clear();
        //                        Dbg.Print(string.Format("request container for item {0} for type {1}", _item.Id, (ItemType)_item.Type), "RPC");
        //                        MmoEngine.Get.Game.Avatar.RequestContainer(_item.Id, (ItemType)_item.Type);
        //                    }
                        }
                    }
                }
            }
        }
    }


}

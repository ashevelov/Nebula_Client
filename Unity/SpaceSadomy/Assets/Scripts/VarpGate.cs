using UnityEngine;
using System.Collections;
using Nebula;

public class VarpGate : MonoBehaviour {

    public string worldName;
    private Collider _other;


    void OnTriggerEnter(Collider other) {
        _other = other;
        if (_other.GetComponent<BaseSpaceObject>())
        {
            if (_other.GetComponent<BaseSpaceObject>().Item.IsMine)
            {
                Invoke("ChangeWorld", 1.5f);
            }
        }
    }

    private void ChangeWorld() {
        if (_other.GetComponent<BaseSpaceObject>())
        {
            BaseSpaceObject baseSpaceObject = _other.GetComponent<BaseSpaceObject>();
            if (baseSpaceObject.GetType() == typeof(MyPlayer)) {

                MyPlayer myPlayer = baseSpaceObject as MyPlayer;
                print(myPlayer.Item);
            }

            if (baseSpaceObject.Item != null)
            {
                if (baseSpaceObject.Item.IsMine)
                {
                    var game = MmoEngine.Get.Game;
                    //game.Avatar.ChangeWorld(worldName);
                    //function moved
                    NRPC.ChangeWorld(worldName);
                }
            }
            else {
                print("item null, wtf??");
            }
        }
    }
}

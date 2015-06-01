using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using System.Collections.Generic;
using Nebula;
using Nebula.UI;

public class Station : MonoBehaviour
{

    private GameObject uiInstance;

    public virtual void OnEnable()
    {
        if (MainCanvas.ObjectIconsEnabled())
        {
			GameObject station=  GameObject.Find("Station");
			if(station !=null)
			{
				Destroy(station);
			}
            var uiPrefab = MainCanvas.Get.GetMiscPrefab("Prefabs/UI/LocalScreen_Object_View");
            uiInstance = Instantiate(uiPrefab);
            uiInstance.GetComponent<Nebula.UI.LocalScreenObjectView>().Setup(this.transform, "Station", ItemTypeName.STATION, () => {
                ConfirmationDialog.Setup("Go to station?", () =>
                {
                    G.Game.EnterWorkshop(WorkshopStrategyType.Angar);
                });
            });
        }
    }

    //void OnDisable()
    //{
    //    //this.screenView.Release();
    //}
    //private float activateDistance = 80;



    //void Update()
    //{
    //    if(G.Game == null )
    //    {
    //        return;
    //    }
    //    if (G.Game.Avatar == null)
    //    {
    //        Dbg.Print("Avatar is null", "NPC");
    //        return;
    //    }
    //    if (!G.Game.Avatar.Component)
    //    {
    //        Dbg.Print("Avatar component is null", "NPC");
    //        return;
    //    }
    //    if (G.Game.Avatar.ShipDestroyed)
    //    {
    //        Dbg.Print("Avatar ship destroyed", "NPC");
    //        return;
    //    }
    //    float distance = Vector3.Distance(G.Game.Avatar.Component.Position, transform.position);
    //    if (distance < this.activateDistance)
    //    {
    //        enterToStation = true;
    //        if(Input.GetKeyDown(KeyCode.E))
    //        {
    //            G.Game.TryEnterWorkshop(WorkshopStrategyType.Angar);
    //        }
    //    }
    //    else
    //    {
    //        enterToStation = false;
    //    }
    //}

    //private bool enterToStation;

    //void OnGUI()
    //{
    //    if (enterToStation)
    //    {
    //        Utils.SaveMatrix();
    //        if (GUI.Button(new Rect(Screen.width /2 / Utils.GameMatrix().m00 - 50, Screen.height / Utils.GameMatrix().m00 - 300, 300, 50), "<size=25>Enter to station</size>", GUI.skin.label))
    //        {
    //            G.Game.TryEnterWorkshop( WorkshopStrategyType.Angar);
    //        }

    //        Utils.RestoreMatrix(); ;
    //    }
    //}

    
    //void OnDrawGizmos()
    //{
    //    if(Application.isPlaying)
    //    {
    //        Gizmos.DrawWireSphere(transform.position, this.activateDistance);
    //    }
    //}
}

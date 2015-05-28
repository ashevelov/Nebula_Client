using Common;
using UnityEngine;
using System.Text;
using Game.Space;
using System.Collections;
using Game.Network;
using Nebula;
using Nebula.UI;
using System;

public class Asteroid : BaseSpaceObject {

    private float updateContentTime;

    private float activateDistance;
    public override void Start()
    {
        base.Start();
        this.activateDistance = G.Game.Settings.Inputs.GetValue<float>("ASTEROID_ACTIVATE_DISTANCE", 0f);
    }


    private void OnRightMouseClickOnScreenView()
    {
        print("Right mouse click on Asteroid: {0}".f(this.Item.Id));
        if(G.Game.Avatar == null)
        {
            Dbg.Print("Avatar is null", "NPC");
            return;
        }
        if(!G.Game.Avatar.Component)
        {
            Dbg.Print("Avatar component is null", "NPC");
            return;
        }
        if(G.Game.Avatar.ShipDestroyed)
        {
            Dbg.Print("Avatar ship destroyed", "NPC");
            return;
        }
        if(this.Item == null )
        {
            Dbg.Print("Asteroid item is null", "NPC");
            return;
        }
        float distance = Vector3.Distance(G.Game.Avatar.Component.Position, this.Position);
        
        if(distance > this.activateDistance)
        {
            Dbg.Print("Your on distance {0:F1}, but need at least {1:F1}. Need closer to asteroid".f(distance, this.activateDistance), "NPC");
            G.Game.ServiceMessageReceiver.AddMessage(new Hashtable 
            { 
                { GenericEventProps.type, ServiceMessageType.Error.toByte() }, 
                { GenericEventProps.message, "Your on distance {0:F1}, but need at least {1:F1}. Need closer to asteroid".f(distance, this.activateDistance) } 
            });
            return;
        }

        MainCanvas.Get.Show(CanvasPanelType.InventorySourceView, (this.Item as AsteroidItem));
    }


    public override void Update()
    {
        if (this.Item != null)
        {
            this.UpdateProperties();
        }
        base.Update();
    }

    public override void OnDestroy() {
        Debug.LogFormat("Asteroid {0} OnDestroy()", this.Item.Id);
    }


    void OnTriggerEnter(Collider other)
    {
        print("Asteroid.OnTriggerEnter()");
        //var bso = other.GetComponent<BaseSpaceObject>();
        //if (bso)
        //{
        //    if (bso.Item.IsMine && (bso.Item.ShipDestroyed == false))
        //    {
        //        if (UIManager.Get)
        //        {
        //            UIManager.Get.InfoActionText.Setup(this.Item.Id, this.GetInfoText(), "TAKE", KeyCode.E, () => {
        //                if (G.Game.Avatar != null) {
        //                    if (false == G.Game.Avatar.ShipDestroyed) {
        //                        G.Game.RequestMoveAsteroidToInventory(this.Item.Id);
        //                    }
        //                }
        //            });
        //        }
        //    }
        //}
    }



    private string GetInfoText()
    {
        AsteroidItem aItem = this.Item as AsteroidItem;
        System.Text.StringBuilder sb = new StringBuilder();
        sb.AppendLine("ASTEROID: " + aItem.Name);
        foreach (var c in aItem.Content)
        {
            sb.AppendLine(string.Format("{0}:{1}-{2}", c.Material.MaterialType, c.Material.Name, c.Count));
        }
        return sb.ToString();
    }

    void OnTriggerExit(Collider other)
    {
        //var bso = other.GetComponent<BaseSpaceObject>();
        //if (bso)
        //{
        //    if (bso.Item.IsMine)
        //    {
        //        if (UIManager.Get)
        //        {
        //            UIManager.Get.InfoActionText.Reset(this.Item.Id);
        //        }
        //    }
        //}
    }

    private float additionalContentUpdate;

    private void UpdateProperties()
    {
        if (Time.time > this.updateContentTime)
        {
            this.updateContentTime = Time.time + 10.0f;
            this.Item.GetProperties(new string[]{GroupProps.ASTEROID});
        }
        if(Time.time > this.additionalContentUpdate)
        {
            this.additionalContentUpdate = Time.time + 1;
            this.Item.GetProperties(new string[] { GroupProps.event_info, GroupProps.DEFAULT_STATE });
        }
    }

    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.position, this.activateDistance);
        }
    }
}

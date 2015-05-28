using Game.Space;
using UnityEngine;
using Common;
using System.Collections;
using Nebula;

public class WorldActivator : BaseSpaceObject 
{
    public override void Start()
    {
        base.Start();
        //this.nextUpdateTime = Time.time;
    }

    //public override void InitializeUI()
    //{
        
    //}

    public override void Update()
    {
        base.Update();
    }

    public override void OnDestroy()
    {
        "activator OnDestroy()".Print();
    }

    void OnTriggerEnter(Collider other)
    {
        //WorldActivatorItem wItem = this.Item as WorldActivatorItem;
        //if (wItem.Active)
        //{
        //    var cOther = other.GetComponent<BaseSpaceObject>();
        //    if (cOther && cOther.Item.IsMine)
        //    {
        //        G.UI.SetActivatorActionViewData(this.Item.Id, "Collect [E]");
        //    }
        //}
    }

    void OnTriggerExit(Collider other)
    {
        //WorldActivatorItem wItem = this.Item as WorldActivatorItem;
        //G.UI.ResetActivatorActionViewData(this.Item.Id); ;
    }

    public override void OnDisable()
    {
        //if(G.UI)
        //    G.UI.ResetActivatorActionViewData(this.Item.Id);

        base.OnDisable();
    }
}


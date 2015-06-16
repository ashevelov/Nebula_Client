using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using Game.Space.UI;
using Nebula;

public class Chest : BaseSpaceObject {

    private float nextUpdateTime;
    private float activateDistance = 80;

    public override void Start()
    {
        base.Start();
        this.nextUpdateTime = Time.time;
    }

    //public override void InitializeUI()
    //{
    //    if (screenView == null) {
    //        screenView = new Game.Space.ObjectScreenSelection();
    //    }

    //    screenView.Initialize(this, Item, "Textures/Icons/container", true, Vector2.one * 25, 
    //        ()=>{},  
    //        () => {
    //        if (Item != null && (false == G.UI.ContainerView.Visible) ) 
    //        {
    //            Debug.Log("Request Chest container");
    //            G.UI.InventorySourceContentView.Show((this.Item as IInventoryItemsSource), Utils.WorldPos2ScreenRect(transform.position, Vector2.one).center);
    //        }
    //    });
    //}
    public override void Update()
    {
        base.Update();

        
        if(Time.time > this.nextUpdateTime)
        {
            if(G.Game.Avatar != null && !G.Game.Avatar.IsDestroyed)
            {
                this.nextUpdateTime = Time.time + 2;
                NRPC.RequestContainer(Item.Id, (ItemType)Item.Type);
            }
        }
    }

    private void CheckPosition()
    {
        if (G.Game == null)
        {
            return;
        }
        if (G.Game.Avatar == null)
        {
            return;
        }
        if (!G.Game.Avatar.Component)
        {
            return;
        }
        if (G.Game.Avatar.ShipDestroyed)
        {
            return;
        }
        float distance = Vector3.Distance(G.Game.Avatar.Component.Position, transform.position);
        if (distance < this.activateDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                G.Game.EnterWorkshop(WorkshopStrategyType.Angar);
            }
        }
    }

    public override void OnDestroy()
    {
        
    }
}

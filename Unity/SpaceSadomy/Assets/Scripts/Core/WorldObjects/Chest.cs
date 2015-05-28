using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using Game.Space.UI;

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
                this.nextUpdateTime = Time.time + 1;
                G.Game.Avatar.RequestContainer(this.Item.Id, this.Item.Type.toItemType());
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
            Dbg.Print("Avatar is null", "NPC");
            return;
        }
        if (!G.Game.Avatar.Component)
        {
            Dbg.Print("Avatar component is null", "NPC");
            return;
        }
        if (G.Game.Avatar.ShipDestroyed)
        {
            Dbg.Print("Avatar ship destroyed", "NPC");
            return;
        }
        float distance = Vector3.Distance(G.Game.Avatar.Component.Position, transform.position);
        if (distance < this.activateDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                G.Game.TryEnterWorkshop(WorkshopStrategyType.Angar);
            }
        }
    }

    public override void OnDestroy()
    {
        
    }
}

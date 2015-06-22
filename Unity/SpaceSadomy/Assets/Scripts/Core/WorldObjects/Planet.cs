using UnityEngine;
using System.Collections;
using Common;
using Game.Space;
using System.Collections.Generic;
using Nebula.Mmo.Items;

public class Planet : BaseSpaceObject
{
    private float activateDistance = 1320;

    //private GameObjectScreenSelection screenView = new GameObjectScreenSelection();
    private float updatePropertiesNextTime;

    void Start()
    {
        this.updatePropertiesNextTime = Time.time;
    }


    //public override void InitializeUI()
    //{
    //    if (this.screenView == null)
    //        this.screenView = new ObjectScreenSelection();

    //    this.screenView.Initialize(this, this.Item, "UI/Textures/Object_icons/station", true, Vector2.one * 50, () =>
    //    {
    //    }, 
    //    () =>
    //    {
    //        Debug.Log("<color=orange>Right click on planet</color>");

    //        ContextMenuView.Setup(new Dictionary<string, System.Action> {
    //            {"Enter to PW1", () => 
    //                {
    //                    var zone = DataResources.Instance.ZoneForId("PW1");
    //                    if(G.Game.World.Name != zone.Id() )
    //                    {
    //                        G.Game.ChangeWorld(zone.Id());
    //                    }
    //                }
    //            },
    //                            {"Enter to PW2", () => 
    //                {
    //                    var zone = DataResources.Instance.ZoneForId("PW2");
    //                    if(G.Game.World.Name != zone.Id() )
    //                    {
    //                        G.Game.ChangeWorld(zone.Id());
    //                    }
    //                }
    //            },
    //                            {"Enter to PW3", () => 
    //                {
    //                    var zone = DataResources.Instance.ZoneForId("PW3");
    //                    if(G.Game.World.Name != zone.Id() )
    //                    {
    //                        G.Game.ChangeWorld(zone.Id());
    //                    }
    //                }
    //            },
    //                            {"Enter to PW4", () => 
    //                {
    //                    var zone = DataResources.Instance.ZoneForId("PW4");
    //                    if(G.Game.World.Name != zone.Id() )
    //                    {
    //                        G.Game.ChangeWorld(zone.Id());
    //                    }
    //                }
    //            },
    //        });
    //    });
    //}


    
    void Update()
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

        /*
        float distance = Vector3.Distance(G.Game.Avatar.Component.Position, transform.position);
        if (distance < this.activateDistance)
        {
            enterToStation = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                G.Game.TryEnterWorkshop(WorkshopStrategyType.Planet);
            }
        }
        else
        {
            enterToStation = false;
        }
        */

        this.UpdateProperties();
    }

    private void UpdateProperties()
    {
        if(this.Item == null)
        {
            return;
        }

        if(Time.time > this.updatePropertiesNextTime)
        {
            this.updatePropertiesNextTime = Time.time + 10f;
            this.Item.GetProperties();
        }
    }

    public override void OnDestroy()
    {
        Debug.Log("planet {0} OnDestroy()".f(((PlanetItem)this.Item).PlanetInfo().Id));
    }

    //private bool enterToStation;

    void OnGUI()
    {
        /*
        if (enterToStation)
        {
            Utils.SaveMatrix();
            if (GUI.Button(new Rect(Screen.width / 2 / Utils.GameMatrix().m00 - 50, Screen.height / Utils.GameMatrix().m00 - 300, 300, 50), "<size=25>Land on the planet (E)</size>", GUI.skin.label))
            {
                G.Game.TryEnterWorkshop(WorkshopStrategyType.Planet);
            }

            Utils.RestoreMatrix(); ;
        }*/
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, this.activateDistance);
    }
}

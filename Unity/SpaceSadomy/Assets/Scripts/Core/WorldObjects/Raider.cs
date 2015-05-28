using UnityEngine;
using System.Collections;
using Common;

public class Raider : BaseSpaceObject {
    private float _updatePropertiesNextTime;

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {

    }

    public override void Update()
    {
        if (Item != null)
        {
            /*
            if (Item.Position != null)
                transform.position = Item.Position.toVector();
            if (Item.Rotation != null)
                transform.rotation = Quaternion.Euler(Item.Rotation.toVector());
             */
            UpdateProperties();
        }
        base.Update();
    }


    private void UpdateProperties()
    {
        if (Time.time > _updatePropertiesNextTime)
        {
            _updatePropertiesNextTime = Time.time + 1.0f;
            Item.GetProperties(new string[] { GroupProps.SHIP_BASE_STATE, GroupProps.SHIP_WEAPON_STATE,  GroupProps.DEFAULT_STATE });
        }
    }
}

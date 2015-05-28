using UnityEngine;
using System.Collections;
using Game.Space;
using Common;

public class StandardNpcCombatObject : BaseSpaceObject {

    private float updatePropertiesNextTime;
    private float defaultPropertiesUpdateInterval = 1.0f;
    private float updateDefaultPropertiesLastTime;

    public override void Start()
    {
        base.Start();
        this.updateDefaultPropertiesLastTime = Time.time;
    }

    public override void OnDestroy()
    {
        
    }

    public override void Update()
    {
        if(this.Item != null )
        {
            this.UpdateProperties();
        }
        base.Update();
    }



    private void UpdateProperties()
    {
        float curTime = Time.time;

        if(curTime > this.updatePropertiesNextTime)
        {
            this.updatePropertiesNextTime = Time.time + 5.0f;
            this.Item.GetProperties(new string[] { "me" } );
        }

        if(curTime > this.updateDefaultPropertiesLastTime + this.defaultPropertiesUpdateInterval)
        {
            this.updateDefaultPropertiesLastTime = curTime;
            this.Item.GetProperties(new string[] { "default", GroupProps.target_info, GroupProps.event_info,
                GroupProps.DEFAULT_STATE});
        }
    }
}

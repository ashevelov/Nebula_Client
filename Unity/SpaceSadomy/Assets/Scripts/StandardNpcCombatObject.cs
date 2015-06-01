using UnityEngine;
using System.Collections;
using Game.Space;
using Common;

public class StandardNpcCombatObject : BaseSpaceObject {

    private float updatePropertiesNextTime;
    private float defaultPropertiesUpdateInterval = 2.0f;
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

        if(curTime > this.updateDefaultPropertiesLastTime + this.defaultPropertiesUpdateInterval)
        {
            this.updateDefaultPropertiesLastTime = curTime;
            this.Item.GetProperties();
        }
    }
}

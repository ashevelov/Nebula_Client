using UnityEngine;
using System.Collections;
using Common;
using Game.Space;

public class DummyEnemy : BaseSpaceObject {

    private float updatePropertiesNextTime;

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        Dbg.Print("DummyEnemy.OnDestroy()");
        
        //this.DestroySkillAndBonusesEffects();
    }

    public override void Update()
    {
        if (this.Item != null)
        {
            this.UpdateProperties();
        }
        base.Update();
    }

    private void UpdateProperties()
    {
        if (Time.time > this.updatePropertiesNextTime)
        {
            this.updatePropertiesNextTime = Time.time + 5.0f;
            this.Item.GetProperties();
        }
    }

 
}

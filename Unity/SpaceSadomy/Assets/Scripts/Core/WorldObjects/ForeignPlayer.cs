using UnityEngine;
using System.Collections;
using Common;

public class ForeignPlayer : BaseSpaceObject {

    private float _updatePropertiesNextTime;

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        //Debug.Log("ForeignPlayer.OnDestroy()");
        //this.DestroySkillAndBonusesEffects();
    }

    public override void Update()
    {
        base.Update();
    }

}

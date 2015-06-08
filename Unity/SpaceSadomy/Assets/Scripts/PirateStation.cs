//using UnityEngine;
//using System.Collections;
//using Common;

//public class PirateStation : BaseSpaceObject 
//{
//    private float updatePropsTime;

//    public override void Start()
//    {
//        base.Start();
//        this.updatePropsTime = Time.time;
//    }

//    public override void OnDestroy()
//    {
        
//    }

//    public override void Update()
//    {
//        if(this.Item != null )
//        {

//        }
//        base.Update();
//    }

//    private void UpdateProperties()
//    {
//        if(Time.time > this.updatePropsTime)
//        {
//            this.updatePropsTime = Time.time + 2;
//            this.Item.GetProperties();
//        }
//    }
//}

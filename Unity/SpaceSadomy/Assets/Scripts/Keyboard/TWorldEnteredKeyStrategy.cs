//using UnityEngine;
//using System.Collections;

//public class TWorldEnteredKeyStrategy : KeyStrategy 
//{

//    private int nextSpaceSceneIndex;

//    public TWorldEnteredKeyStrategy(string id)
//        : base(id)
//    { 
        
//    }

//    public override void HandleDown()
//    {
//        if(G.CurrentWorldId() != "1" )
//        {
//            Debug.LogWarning("Invalid world for changing backgrounds");
//            return;
//        }
//        if(this.NextSpaceSceneIndex() >= 17)
//        {
//            this.SetNextSpaceIndex(0);
//        }
//        string sceneName = "SpaceScene" + this.NextSpaceSceneIndex().ToString();
//        SU_SpaceSceneSwitcher.Switch(sceneName);
//        this.SetNextSpaceIndex(this.NextSpaceSceneIndex() + 1);
//    }

//    private void SetNextSpaceIndex(int index)
//    {
//        this.nextSpaceSceneIndex = index;
//    }

//    private int NextSpaceSceneIndex()
//    {
//        return this.nextSpaceSceneIndex;
//    }
//}

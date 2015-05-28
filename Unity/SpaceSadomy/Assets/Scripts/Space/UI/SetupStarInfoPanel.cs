//namespace Game.Space.UI {
//    using UnityEngine;
//    using Photon.Mmo.Client;
//    using Game.Space.Resources;
//    using System.Collections;
//    using Nebula;

//    public static class SetupStarInfoPanel
//    {

//        private static WorldEventData _selectedWorldEventData = null;

//        public static void Setup(StarSystem star, bool warp, SpaceZone curentZone)
//        {
//            UIEntity panel = UIManager.Get.GetLayout("star_info");
//            panel.SetVisibility(true);

//            var scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;

//            var world = MmoEngine.Get.Game.World;

//            Debug.Log("world name = " + world.Name);

//            var statusLabel = panel.GetChildrenEntityByName("fraction") as UILabel;
//            statusLabel._text = star._control.ToString();

//            scrollView.Clear();
////            star._spaceZone.ForEach((z) =>
////            {
////                var template = scrollView.CreateElement();
////                var nameLabel = template.GetChildrenEntityByName("name") as UILabel;
////                nameLabel._text = z._zoneId;
////                UIButton btn = template.GetChildrenEntityByName("warp") as UIButton;
////                btn.tag = z._zoneId;
////                btn._enabled = warp;
////
////                var descritionLabel = template.GetChildrenEntityByName("description") as UILabel;
////                descritionLabel._text = z._zoneDesc + " This solar system is controlled by " + star._control.ToString();
////                scrollView.AddChild(template);
////            });

//            UIManager.Get.RegisterEventHandler("STAR_INFO_CLOSE_CLICK", null, (evt) =>
//            {
//                panel.SetVisibility(false);
//            });

//            UIManager.Get.UnregisterEventHandler("WARP_TO_ZONE");

//            UIManager.Get.RegisterEventHandler("WARP_TO_ZONE", null, (evt) =>
//            {
////                string id = evt._sender.tag as string;
////                panel.SetVisibility(false);
////                MapController.Hide();
////
////                MyPlayer myPlayer = GameObject.FindObjectOfType<MyPlayer>();
////                if (myPlayer != null)
////                {
////                    Vector3 direction = (star._spaceZone.Find((z) => { return (z._zoneId == evt._sender.tag); }).pos - curentZone.pos) * 1000000;
////                    myPlayer.MoveToDirection(direction);
////                }
////                GameObject go = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Warp")) as GameObject;
////                go.transform.parent = myPlayer.transform;
////                go.transform.localRotation = Quaternion.identity;
////                Debug.Log(go.transform.rotation);
////                GameObject.Destroy(go, 7);

                
//                var game = MmoEngine.Get.Game;

//                string worldNameId = DataResources.Instance.ZoneForScene(evt._sender.tag.ToString()).Id(); //MmoEngine.WorldForLevel(evt._sender.tag.ToString());

//                if (game.World.Name == worldNameId)
//                {
//                    Debug.Log("you already in this world");
//                }
//                else
//                {
//                    //game.Avatar.ChangeWorld(worldNameId);
//                    //function moved
//                    NRPC.ChangeWorld(worldNameId);
//                }
//                Debug.Log("warp -> " + worldNameId);
//            });


//            UIManager.Get.UnregisterEventHandler("STAR_INFO_ROUTE");

//            UIManager.Get.RegisterEventHandler("STAR_INFO_ROUTE", null, (evt) =>
//            {
//                if (MapController.MapExist())
//                {
//                    MapController.Get.SetRoute(star);
//                }
//            });

//        }
//    }
//}

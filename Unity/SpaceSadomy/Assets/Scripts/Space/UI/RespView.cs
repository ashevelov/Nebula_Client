//// RespView.cs
//// Nebula
//// 
//// Created by Oleg Zhelestcov on Wednesday, November 5, 2014 2:13:28 PM
//// Copyright (c) 2014 KomarGames. All rights reserved.
////
//using UnityEngine;
//using System.Collections;
//using Nebula;

//namespace Game.Space.UI
//{
//    public class RespView 
//    {
//        private UIContainerEntity root;

//        public RespView(UIManager manager)
//        {
//            root = manager.GetLayout("resp") as UIContainerEntity;

//            root.SetOnKeyDownHandler(code =>
//                {
//                    if (this.Visible)
//                    {
//                        if (code == KeyCode.E)
//                        {
//                            NRPC.Respawn();
//                        }
//                    }
//                });

//            (root.GetChildrenEntityByName("resp_button") as UIButton).RegisterHandler(evt =>
//                {
//                    if (this.Visible)
//                    {
                       
//                        NRPC.Respawn();
//                    }
//                });
//        }

//        public bool Visible
//        {
//            get
//            {
//                return this.root.Visible;
//            }
//        }

//        public void SetVisible(bool vis)
//        {
//            this.root.SetVisibility(vis);
//        }
//    }
//}

//// EventObjectScreenSelection.cs
//// Nebula
//// 
//// Created by Oleg Zhelestcov on Tuesday, December 2, 2014 4:28:48 PM
//// Copyright (c) 2014 KomarGames. All rights reserved.
////
//namespace Game.Space
//{
//    using UnityEngine;
//    using System.Collections;
//    using Game.Network;
//    using Nebula;

//    public class EventObjectScreenSelection : ISpaceObjectGUI
//    {
//        private EventSpaceObject eventObject;
//        private Vector2 iconSize;
//        private Texture2D icon;

//        private Rect drawRect;
//        private bool initialized = false;

//        public EventObjectScreenSelection()
//        {

//        }

//        public void Initialize(EventSpaceObject eventObject, Vector2 iconSize)
//        {
//            if (!this.initialized)
//            {
//                this.eventObject = eventObject;
//                this.iconSize = iconSize;
//                this.icon = TextureCache.Get("UI/Textures/Object_icons/ivent");

//                if (G.UI)
//                    G.UI.AddSpaceObjectGui(this);

//                this.initialized = true;
//            }
//        }

//        public void Draw()
//        {
//            if (this.initialized)
//            {
//                if (!NetworkGame.OperationsHelper.GuiHided && !MapController.MapExist())
//                {
//                    if (Event.current.type == EventType.Repaint)
//                    {
//                        Color oldColor = GUI.color;
//                        GUI.color = Color.yellow;
//                        if (icon)
//                        {
//                            GUI.DrawTexture(this.drawRect, this.icon);
//                        }
//                        GUI.color = oldColor;
//                    }
//                }
//            }
//        }

//        public void Update()
//        {
//            if (this.initialized)
//            {
//                if (!NetworkGame.OperationsHelper.GuiHided && this.eventObject && this.eventObject.gameObject && this.eventObject.gameObject.activeSelf)
//                {
//                    this.drawRect = Utils.WorldPos2ScreenRect(this.eventObject.transform.position, this.iconSize);
//                }
//            }
//        }

//        public GameObject GetObject()
//        {
//            if (this.eventObject != null)
//                return this.eventObject.gameObject;
//            return null;
//        }

//        public void Release()
//        {
//            if (G.UI)
//                G.UI.RemoveSpaceObjectGui(this);
//        }
//    }
//}

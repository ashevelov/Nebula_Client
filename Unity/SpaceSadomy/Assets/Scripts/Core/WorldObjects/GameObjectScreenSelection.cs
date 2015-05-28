//namespace Game.Space
//{
//    using UnityEngine;
//    using System.Collections;
//    using Game.Network;
//    using Nebula;

//    public class GameObjectScreenSelection : ISpaceObjectGUI
//    {
//        private GameObject gameObject;
//        private Vector2 iconSize;
//        private Texture2D icon;

//        private Rect drawRect;
//        private bool initialized = false;

//        public GameObjectScreenSelection()
//        {

//        }

//        public void Initialize(GameObject gameObject, Vector2 iconSize, Texture2D icon)
//        {
//            if (!this.initialized)
//            {
//                this.gameObject = gameObject;
//                this.iconSize = iconSize;
//                this.icon = icon;

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
//                        GUI.color = Color.white;
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
//                if (!NetworkGame.OperationsHelper.GuiHided && this.gameObject && this.gameObject.activeSelf)
//                {
//                    this.drawRect = Utils.WorldPos2ScreenRect(this.gameObject.transform.position, this.iconSize);
//                }
//            }
//        }

//        public GameObject GetObject()
//        {
//            return this.gameObject;
//        }

//        public void Release()
//        {
//            if (G.UI)
//                G.UI.RemoveSpaceObjectGui(this);
//        }
//    }
//}

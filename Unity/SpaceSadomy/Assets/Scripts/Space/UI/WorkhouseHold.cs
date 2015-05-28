//using Common;
//using System.Collections.Generic;
//using UnityEngine;
//using Nebula;
//using Nebula.Client;

//namespace Game.Space.UI
//{
//    public class WorkhouseHold 
//    {
//        private const int NUM_COLS = 6;
//        private const int NUM_ROWS = 12;

//        private UIManager uiManager;
//        private UIContainerEntity root;
//        private Dictionary<string, Texture2D> cachedTextures;
//        private IStationHoldableObject selectedInfo;
//        private List<IStationHoldableObject> holdObjects;

//        private UIButton closeButton;
//        private int startViewIndex;
//        private Texture2D transparentTexture;


//        private int GetListIndex(string btnName)
//        {
//            return this.startViewIndex + int.Parse(btnName);
//        }

//        private Texture2D GetInfoTexture(IStationHoldableObject info)
//        {
//            ClientShipModule csModele = info as ClientShipModule;
//            //string race;
//            //string workshop;
//            //string set;
//            //string slot;
//            string id = csModele.prefab.Remove(0, 22);

//            return TextureCache.Get("UI/Textures/Modules/"+id);
//        }
		
//		private string GetInfoText(IStationHoldableObject info)
//		{
//			ClientShipModule csModele = info as ClientShipModule;

//			switch(csModele.type)
//			{
//				case ShipModelSlotType.CB:
//				return "C";
//			case ShipModelSlotType.ES:
//				return "E";
//			case ShipModelSlotType.DF:
//				return "G";
//			case ShipModelSlotType.DM:
//				return "S";
//			case ShipModelSlotType.CM:
//				return "R";
//			}

//			return "";
//		}
		
		
//		private Texture2D GetIconColor(IStationHoldableObject info)
//		{
//            ClientShipModule csModele = info as ClientShipModule;
//            Debug.Log("csModele.color = " + csModele.color);

//			return TextureCache.Get("UI/Textures/item_color_"+csModele.color);
//		}

//        public WorkhouseHold(UIManager uiManager)
//        {
//            this.uiManager = uiManager;
//            this.root = this.uiManager.GetLayout("station_hold") as UIContainerEntity;
//            this.closeButton = this.root.GetChildrenEntityByName("close") as UIButton;
//            this.holdObjects = new List<IStationHoldableObject>();
//            this.root.SetUpdateInterval(0.2f);
//            this.root.RegisterUpdate(Update);
//            this.closeButton.RegisterHandler(OnClose);
//            this.transparentTexture = TextureCache.Get("UI/Textures/transparent");

//            for (int i = 0; i < NUM_COLS * NUM_ROWS; i++)
//            {
//                var b = this.root.GetChildrenEntityByName(i.ToString()) as UIButton;
//                b._style = new GUIStyle(b._style);
//                b.RegisterHandler(UIEventType.Default,      OnLeftClick);
//                b.RegisterHandler(UIEventType.LeftClick,    OnLeftClick);
//                b.RegisterHandler(UIEventType.RightClick,   OnRightClick);
//                b.RegisterHandler(UIEventType.OnEnter, OnMouseEnter);
//                b.RegisterHandler(UIEventType.OnExit, OnMouseExit);
//            }
//            /*
//            this.textures = new Dictionary<StationHoldableObjectType, Texture2D>();
//            this.uiManager = uiManager;
//            this.root = uiManager.GetLayout("station_hold") as UIGroup;
//            this.actionButton = this.root.GetChildrenEntityByName("action_button") as UIButton;
//            this.scroll = this.root.GetChildrenEntityByName("scroll_items") as UIScrollView;
//            this.scroll.SetUpdateInterval(0.5f);
//            this.scroll.RegisterUpdate((p) => { FillScroll(); });

//            this.uiManager.RegisterEventHandler("SH_TOGGLE_CHANGED", null, OnToggleClicked);
//            this.uiManager.RegisterEventHandler("SH_ACTION_BUTTON", null, OnActionButton);
//             */
//        }

//        private int GetMaxStartIndex()
//        {
//            if (this.holdObjects.Count <= NUM_ROWS * NUM_COLS)
//                return 0;
//            return Mathf.FloorToInt(this.holdObjects.Count / NUM_COLS) * NUM_COLS   - NUM_ROWS * NUM_COLS;
//        }

//        private void ClampStartIndex()
//        {
//            if (this.startViewIndex < 0)
//                this.startViewIndex = 0;

//            int maxIndex = this.GetMaxStartIndex();
//            if (this.startViewIndex > maxIndex)
//                this.startViewIndex = maxIndex;
//        }


//        private void PrintObjectsType(List<IStationHoldableObject> list)
//        {
//            System.Text.StringBuilder sb = new System.Text.StringBuilder();
//            foreach (var m in list)
//            {
//                sb.AppendLine(((ClientShipModule)m).type.ToString());
//            }
//            Debug.Log(sb.ToString());
//        }

//        void Update(UIContainerEntity e)
//        {
//            var allOrderedItems = ((ClientWorkhouseStationHold)G.Game.Station.Hold).OrderedItems;
//            var moduleItems = NoLinqUtils.CastToModuleType(NoLinqUtils.FilterByHoldType(allOrderedItems, StationHoldableObjectType.Module)); //allOrderedItems.Where(i => i.Type == StationHoldableObjectType.Module).Cast<ClientShipModule>().ToList();
//            var otherItems = NoLinqUtils.FilterByPredicate(allOrderedItems, (obj) => obj.Type != StationHoldableObjectType.Module); //allOrderedItems.Where(i => i.Type != StationHoldableObjectType.Module).ToList();
//            moduleItems = NoLinqUtils.OrderByType(moduleItems); //moduleItems.OrderBy(m => m.type).ToList();
//            if (this.holdObjects == null)
//                this.holdObjects = new List<IStationHoldableObject>();
//            this.holdObjects.Clear();

//            moduleItems.ForEach((m) => this.holdObjects.Add(m));
//            otherItems.ForEach(i => this.holdObjects.Add(i));

//            //PrintObjectsType(this.holdObjects);

//            for (int i = 0; i < NUM_COLS * NUM_ROWS; i++)
//            {
//                UIButton button = this.root.GetChildrenEntityByName(i.ToString()) as UIButton;
//                UITexture icon = this.root.GetChildrenEntityByName(i.ToString() + "_tex") as UITexture;
//                UILabel label = this.root.GetChildrenEntityByName(i.ToString() + "_label") as UILabel;
//				UITexture item_color = this.root.GetChildrenEntityByName("color_"+i.ToString()) as UITexture;
//                int listIndex = this.GetListIndex(button._name);
//                if (listIndex < this.holdObjects.Count)
//                {

//                    var info = this.holdObjects[listIndex] as ClientShipModule;
//                    if (info != null)
//                    {
//                        button.SetTag(info);
//                        //button._style.SetStyleTexture(this.GetInfoTexture(info));
//                        //button.SetText(this.GetInfoText(info).Color("red"));
//                        label.SetText(this.GetInfoText(info));
//                        //button._style.alignment = TextAnchor.LowerRight;
//                        //button._style.contentOffset = new Vector2(-2,0);
//                        icon.SetTexture(this.GetInfoTexture(info));
//                        //item_color.SetTexture(this.GetIconColor(info));


//                        if (info.workshop != ((byte)G.Game.UserInfo.GetSelectedCharacter().HomeWorkshop).toEnum<Workshop>())
//                        {
//                            button.SetUseCustomColor(true, Color.red);
//                        }
//                        else
//                        {
//                            button.SetUseCustomColor(false, Color.red);
//                        }
//                    }
//                }
//                else
//                {
//                    button.SetTag(null);
//                    //button._style.SetStyleTexture(this.transparentTexture);
//                    icon.SetTexture(this.transparentTexture);
//					button._text = string.Empty;
//                    button.SetUseCustomColor(false, Color.red);
//                }
//            }

//            Vector2 mp = Utils.ConvertedMousePosition;
//            if (this.root.GetGlobalRect.Contains(mp))
//            {
//                if (Input.GetAxis("Mouse ScrollWheel") > 0)
//                {
//                    this.startViewIndex += NUM_COLS;
//                    this.ClampStartIndex();
//                    Debug.Log("+new start index: {0}".f(this.startViewIndex));
//                }
//                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
//                {
//                    this.startViewIndex -= NUM_COLS;
//                    this.ClampStartIndex();
//                    Debug.Log("-new start index: {0}".f(this.startViewIndex));
//                }
//            }
//        }

//        void OnClose(UIEvent e)
//        {
//            this.SetVisible(false);
//        }

//        public bool Visible
//        {
//            get
//            {
//                return this.root.Visible;
//            }
//        }

//        public void SetVisible(bool b)
//        {
//            this.root.SetVisibility(b);
//        }

//        void OnRightClick(UIEvent e)
//        {
//            if (e._sender.tag != null)
//            {
//                var info = e._sender.tag as ClientShipModule;
//                if (info.workshop == ((byte)G.Game.UserInfo.GetSelectedCharacter().HomeWorkshop).toEnum<Workshop>())
//                {
//                    NRPC.EquipModuleFromHoldToShip(info.Id);
//                    G.UI.ObjectInfoView.Close();
//                }
//            }
//        }

//        void OnLeftClick(UIEvent e)
//        {
            
//            //Input.
//        }

//        void OnMouseEnter(UIEvent evt)
//        {
//            IStationHoldableObject obj = evt._sender.tag as IStationHoldableObject;
//            if (obj != null)
//            {
//                switch (obj.Type)
//                {
//                    case StationHoldableObjectType.Module:
//                        {
//                            ClientShipModule playerModule = G.Game.Ship.ShipModel.Module(((ClientShipModule)obj).type);
//                            G.UI.ObjectInfoView.ShowModule((ClientShipModule)obj, playerModule, Utils.ConvertedMousePosition.x, Utils.ConvertedMousePosition.y, 400, 700, true);
//                        }
//                        break;
//                }
//            }
//        }

//        void OnMouseExit(UIEvent e)
//        {
//            if (G.UI.ObjectInfoView.Visible)
//            {
//                if (G.UI.ObjectInfoView.CurrentObjectType == UIObjectInfoView.ObjectType.Module)
//                {
//                    IStationHoldableObject obj = e._sender.tag as IStationHoldableObject;
//                    if (obj != null)
//                    {
//                        if (obj.Type == StationHoldableObjectType.Module)
//                        {
//                            if (obj.Id == G.UI.ObjectInfoView.CurrentObjectId)
//                            {
//                                G.UI.ObjectInfoView.Close();
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        /*
//        private void FillScroll()
//        {
//            this.scroll.Clear();
//            foreach (var it in MmoEngine.Get.Game.Station.Hold.Items)
//            {
//                foreach (var it2 in it.Value)
//                {
//                    IStationHoldableObject item = it2.Value;
//                    var template = this.scroll.CreateElement();
//                    var icon = template.GetChildrenEntityByName("icon") as UITexture;
//                    icon.SetTexture(TextureForItem(item));
//                    var typeLabel = template.GetChildrenEntityByName("type") as UILabel;
//                    typeLabel.SetText(TypeTextForItem(item));
//                    var nameLabel = template.GetChildrenEntityByName("name") as UILabel;
//                    nameLabel.SetText(NameTextForItem(item));

//                    var toggle = template.GetChildrenEntityByName("select_item") as UIToggle;
//                    toggle.SetTag(item);
//                    toggle.SetMouseRightButtonUpAction(OnToggleMouseRightUp);
//                    toggle.SetMouseLeftButtonUpAction(OnToggleMouseLeftUp);

//                    if (GetSelectedId() == item.Id)
//                    {
//                        toggle.SetValue(true);
//                    }
//                    else
//                    {
//                        toggle.SetValue(false);
//                    }

//                    this.scroll.AddChild(template);
//                }
//            }

            
//        }

//        private Texture2D TextureForItem(IStationHoldableObject obj)
//        {
//            var type = obj.Type;
//            if (this.textures.ContainsKey(type))
//            {
//                return this.textures[type];
//            }
//            else
//            {
//                switch (type)
//                {
//                    case StationHoldableObjectType.Module:
//                        {
//                            this.textures[type] = TextureCache.Get("UI/Textures/module");
//                        }
//                        break;
//                    default:
//                        {
//                            this.textures[type] = TextureCache.Get("UI/Textures/red");
//                        }
//                        break;
//                }
//                return this.textures[type];
//            }
//        }

//        private string TypeTextForItem(IStationHoldableObject item)
//        {
//            switch (item.Type)
//            {
//                case StationHoldableObjectType.Module:
//                    return "module";
//                default:
//                    return "TYPE UNKNOWN";
//            }
//        }

//        private string NameTextForItem(IStationHoldableObject item)
//        {
//            switch (item.Type)
//            {
//                case StationHoldableObjectType.Module:
//                    {
//                        ClientShipModule module = item as ClientShipModule;
//                        return module.name + module.Id.Substring(0, 4);
//                    }
//                default:
//                    return "UNKNOWN NAME";
//            }
//        }

//        private string GetSelectedId() {
//            if (selectedInfo == null)
//                return string.Empty;
//            else
//                return selectedInfo.Id;
//        }

//        private void SetSelection(IStationHoldableObject info)
//        {
//            this.selectedInfo = info;
//        }

//        private void OnToggleClicked(UIEvent evt)
//        {
//            if (evt._parameters != null)
//            {
//                bool toggled = evt._parameters.GetValue<bool>("new_value", false);
//                var toggle = evt._sender as UIToggle;
//                if (toggled && toggle != null)
//                {
//                    SetSelection(toggle.tag as IStationHoldableObject);
//                    this.ToggleOffExcept((toggle.tag as IStationHoldableObject).Id);
//                }
//                else
//                {
//                    this.ToggleOffAll();
//                }
//            }
//        }

//        private void OnToggleMouseRightUp(UIEntity e)
//        {
//            if (e is UIToggle && e.tag != null)
//            {
//                var toggle = e as UIToggle;
//                uiManager.ContextMenu.Show(toggle.tag);


//            }
//        }

//        private void OnToggleMouseLeftUp(UIEntity e)
//        {
//            uiManager.ContextMenu.SetVisibility(false);
//        }

//        private void OnActionButton(UIEvent evt)
//        {
//            if (this.selectedInfo != null)
//            {
//                MmoEngine.Get.Game.EquipModuleFromHoldToShip(this.selectedInfo.Id);
//                this.selectedInfo = null;
//            }
//        }

//        private void ToggleOffExcept(string infoId)
//        {
//            foreach (var orderedChildrens in this.scroll._childrens)
//            {
//                foreach (var elem in orderedChildrens.Value)
//                {
//                    var toggle = elem.GetChildrenEntityByName("select_item") as UIToggle;
//                    if (toggle != null && toggle.tag != null)
//                    {
//                        if ((toggle.tag as IStationHoldableObject).Id == infoId)
//                            toggle.SetValue(true);
//                        else
//                            toggle.SetValue(false);
//                    }
//                }
//            }
//        }

//        private void ToggleOffAll()
//        {
//            foreach (var orderedChildrens in this.scroll._childrens)
//            {
//                foreach (var elem in orderedChildrens.Value)
//                {
//                    var toggle = elem.GetChildrenEntityByName("select_item") as UIToggle;
//                    if (toggle != null)
//                    {
//                        toggle.SetValue(false);
//                    }
//                }
//            }
//        }
//        */

//    }
//}

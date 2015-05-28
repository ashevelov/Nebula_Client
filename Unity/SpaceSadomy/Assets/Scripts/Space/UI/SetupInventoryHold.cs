//using Common;
//using Nebula.Client.Inventory;
//using Nebula.Client.Inventory.Objects;
//using Nebula.Client.Res;
//using Photon.Mmo.Client;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Nebula.Client;
//using Nebula;

//namespace Game.Space.UI
//{
//    public  class SetupInventoryHold 
//    {
//        private UIManager manager;

//        private GUIStyle cellStyle;
//        //private static List<string> _select_items = new List<string>();

//        private Dictionary<string, Texture2D> textures;

//        private  UIResizeableGroup GetRoot() {
//            return UIManager.Get.GetLayout("inventory_hold") as UIResizeableGroup;
//        }

//        private  UIScrollView GetScrollView(UIEntity root) {
//            return root.GetChildrenEntityByName("scroll_info") as UIScrollView;
//        }


//        public  void ShowContainer() {

//            var root = GetRoot();
//            FillList(root, GetScrollView(root));
//            root.SetVisibility(true);
//        }

//        public  void Clear() {
//            GetScrollView(GetRoot()).Clear();
//        }


//        private  void FillList(UIResizeableGroup group, UIScrollView scrollView) {
//            scrollView.Clear();
//            AddStationItems(scrollView);
//            AddInventoryItems(scrollView);
//        }

//        private void AddInventoryItems(UIScrollView scrollView)
//        {
//            foreach (var item in G.Game.Station.StationInventory.Items)
//            {
//                foreach (var info in item.Value)
//                {
//                    UIScrollItemTemplate template = scrollView.CreateElement();

//                    UITexture tex = template.GetChildrenEntityByName("icon") as UITexture;
//                    UITexture item_color = template.GetChildrenEntityByName("item_color") as UITexture;
//                    if (tex._texture == null || tex._texture.name != info.Value.Object.Type.ToString())
//                    {
//                        //tex.SetTexture(SetupInventory.TextureForItem(info.Value.Object));
//                        //item_color.SetTexture(SetupInventory.GetIconColor(info.Value.Object));
//                    }

//                    Rect cellRect = tex._rect;
//                    UIButton cellButton = new UIButton { _enabled = true, _name = info.Value.Object.Id, _parent = template, _style = cellStyle, _text = string.Empty, _visible = true, align = UIAlign.None, blockMouse = true, selfDrawable = false, zorder = 0, tag = (IInfo)info.Value.Object };
//                    cellButton.SetRect(cellRect, UIUnits.px);
//                    cellButton.events = new List<UIEvent>
//                    {
//                        //new UIEvent { _name = "INVENTORY_VIEW_CELL_LEFT_CLICK_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.LeftClick },
//                        new UIEvent { _name = "INVENTORY_HOLD_VIEW_CELL_RIGHT_CLICK_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.RightClick },
//                        new UIEvent { _name = "INVENTORY_HOLD_VIEW_CELL_ON_MOUSE_ENTER_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.OnEnter },
//                        new UIEvent { _name = "INVENTORY_HOLD_VIEW_CELL_ON_MOUSE_EXIT_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.OnExit }
//                    };

//                    template.AddChild(cellButton);
//                    cellButton.RegisterHandler(UIEventType.RightClick, OnRightClick);
//                    cellButton.RegisterHandler(UIEventType.OnEnter, OnMouseEnter);
//                    cellButton.RegisterHandler(UIEventType.OnExit, OnMouseExit);
//                    UILabel nameLabel = template.GetChildrenEntityByName("name") as UILabel;
//                    nameLabel.SetText(info.Value.Count.ToString());
//                    scrollView.AddChild(template);
//                }
//            }
//        }

//        private List<IStationHoldableObject> FilterByType(List<IStationHoldableObject> inputs, StationHoldableObjectType type) {
//            List<IStationHoldableObject> resultList = new List<IStationHoldableObject>();
//            foreach (var obj in inputs) {
//                if (obj.Type == type) {
//                    resultList.Add(obj);
//                }
//            }
//            return resultList;
//        }

//        private List<ClientShipModule> CastToModule(List<IStationHoldableObject> inputs) {
//            List<ClientShipModule> resultList = new List<ClientShipModule>();
//            foreach (var obj in inputs) {
//                if (obj is ClientShipModule) {
//                    resultList.Add(obj as ClientShipModule);
//                }
//            }
//            return resultList;
//        }

//        private void AddStationItems(UIScrollView scrollView)
//        {

//            var allOrderedItems = ((ClientWorkhouseStationHold)G.Game.Station.Hold).OrderedItems;

//            var its_1 = FilterByType(allOrderedItems, StationHoldableObjectType.Module);
//            var its_2 = CastToModule(its_1);
//            var moduleItems = its_2;
//            foreach (var info in moduleItems)
//            {
//                if (info == null)
//                    Debug.LogError("MODULE INFO NULL");

//                ClientShipModule idt = info;
//                UIScrollItemTemplate template = scrollView.CreateElement();

//                UITexture tex = template.GetChildrenEntityByName("icon") as UITexture;
//                UITexture item_color = template.GetChildrenEntityByName("item_color") as UITexture;
//                item_color.SetTexture(GetIconColor(info));
//                tex.SetTexture(GetInfoTexture(info));


//                Rect cellRect = tex._rect;
//                //creating elements

//                UIButton cellButton = new UIButton { _enabled = true, _name = info.id, _parent = template, _style = cellStyle, _text = string.Empty, _visible = true, align = UIAlign.None, blockMouse = true, selfDrawable = false, zorder = 0, tag = (IInfo)info };
//                cellButton.SetRect(cellRect, UIUnits.px);
//                cellButton.events = new List<UIEvent>
//                    {
//                        //new UIEvent { _name = "INVENTORY_VIEW_CELL_LEFT_CLICK_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.LeftClick },
//                        new UIEvent { _name = "HOLD_VIEW_CELL_RIGHT_CLICK_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.RightClick },
//                        new UIEvent { _name = "HOLD_VIEW_CELL_ON_MOUSE_ENTER_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.OnEnter },
//                        new UIEvent { _name = "HOLD_VIEW_CELL_ON_MOUSE_EXIT_EVENT", _parameters = new Hashtable(), _sender = cellButton, EventType = UIEventType.OnExit }
//                    };

//                template.AddChild(cellButton);
//                cellButton.RegisterHandler(UIEventType.RightClick, OnRightClick);
//                cellButton.RegisterHandler(UIEventType.OnEnter, OnMouseEnter);
//                cellButton.RegisterHandler(UIEventType.OnExit, OnMouseExit);
//                scrollView.AddChild(template);
//            }
//        }

//        public SetupInventoryHold(UIManager manager)
//        {
//            this.manager = manager;
//            textures = new Dictionary<string, Texture2D>();
//            UIResizeableGroup group = UIManager.Get.GetLayout("inventory_hold") as UIResizeableGroup;
//            UIButton closeButton = group.GetChildrenEntityByName("close_button") as UIButton;

//            UIScrollView scrollView = group.GetChildrenEntityByName("scroll_info") as UIScrollView;
//            this.cellStyle = this.manager.GetSkin("game").GetStyle("cell_button");

//            scrollView.SetUpdateInterval(0.5f);
//            scrollView.RegisterUpdate((parent) => {
//                FillList(group, scrollView);
//            });

//            UIManager.Get.RegisterEventHandler("INVENTORY_HOLD_ITEM_SELECT_CLICK", null, (evt) =>
//            {
//                //SetupItemInfo.Setup<ClientShipModule>(evt._sender.tag, GetInfoTexture(evt._sender.tag as IStationHoldableObject));
//            });

//            UIManager.Get.RegisterEventHandler("INVENTORY_HOLD_CLOSE_CLICK", null, (evt) =>
//            {
//                if (group._visible) {
//                    group.SetVisibility(false);
//                }
//            });
//        }

        
//        public  bool Visible {
//            get
//            {
//                UIEntity group = UIManager.Get.GetLayout("inventory_hold");
//                return group._visible;
//            }
//            set
//            {
//                UIEntity group = UIManager.Get.GetLayout("inventory_hold");
//                group._visible = value;
//            }
//        }


//        private Texture2D GetInfoTexture(IStationHoldableObject info)
//        {
//            if(info.Type == StationHoldableObjectType.Module)
//            {
//                ClientShipModule csModele = info as ClientShipModule;
//                string id = csModele.prefab.Remove(0, 22);

//                return TextureCache.Get("UI/Textures/Modules/"+id);
//            }
//            else
//            {
//                return TextureCache.Get("UI/Textures/transparent");
//            }
//        }

//        private Texture2D GetIconColor(IStationHoldableObject info)
//        {
//            ClientShipModule csModele = info as ClientShipModule;

//            return TextureCache.Get("UI/Textures/item_color_" + csModele.color);
//        }


//        void OnRightClick(UIEvent e)
//        {
//            Debug.Log("OnRightClick");

//            showItemInfo = false;
//            //SetupItemInfo.HideWhenMouseExitFromSource();

//            Dictionary<string, System.Action> contextActions = new Dictionary<string, System.Action>();

//            if (e._sender.tag is IInventoryObjectInfo)
//            {
//                IInventoryObjectInfo item = ((IInventoryObjectInfo)e._sender.tag);

//                if (item.Type == InventoryObjectType.Weapon)
//                {
//                    WeaponInventoryObjectInfo wInfo = item as WeaponInventoryObjectInfo;
//                    var weaponTemplate = DataResources.Instance.Weapon(wInfo.Template);
//                    if (weaponTemplate.Workshop == ((byte)G.Game.UserInfo.GetSelectedCharacter().HomeWorkshop).toEnum<Workshop>() && wInfo.Level <= G.Game.UserInfo.GetSelectedCharacter().CharacterLevel)
//                    {
//                        contextActions.Add("Equip", () =>
//                        {
//                            Debug.Log("<color=orange>Equip from station called</color>");
//                            NRPC.EquipWeapon( InventoryType.station, item.Id);
//                            AudioControloller.Get.PlayUISound(GameSoundsType.module_equip);
//                        });
//                    }
//                }
//                if (G.Game.State == GameState.Workshop)
//                {
//                    contextActions.Add("Send to the Inventory", () =>
//                    {
//                        NRPC.MoveItemFromStationToInventory(item.Type, item.Id);
//                    });
//                }

//                if (G.Game.State == GameState.Workshop && item.Type == InventoryObjectType.Scheme)
//                {
//                    SchemeInventoryObjectInfo scheme = item as SchemeInventoryObjectInfo;
//                    ResMaterialData firstMaterial = null;
//                    ResMaterialData secondMaterial = null;
//                    foreach (var pair in scheme.CraftMaterials)
//                    {
//                        if (firstMaterial == null)
//                        {
//                            firstMaterial = DataResources.Instance.OreData(pair.Key);
//                        }
//                        else if (secondMaterial == null)
//                        {
//                            secondMaterial = DataResources.Instance.OreData(pair.Key);
//                        }
//                    }
//                    bool allowedFirst = false;
//                    bool allowedSecond = false;

//                    if (firstMaterial != null)
//                    {
//                        int needCount = scheme.CraftMaterials[firstMaterial.Id];
//                        int playerCount = 0;
//                        ClientInventoryItem cItem;
//                        if (G.Game.Inventory.TryGetItem(InventoryObjectType.Material, firstMaterial.Id, out cItem))
//                        {
//                            playerCount += cItem.Count;
//                        }
//                        if (G.Game.Station.StationInventory.TryGetItem(InventoryObjectType.Material, firstMaterial.Id, out cItem))
//                        {
//                            playerCount += cItem.Count;
//                        }

//                        if (playerCount >= needCount)
//                        {
//                            allowedFirst = true;
//                        }
//                    }
//                    if (secondMaterial != null)
//                    {
//                        int needCount = scheme.CraftMaterials[secondMaterial.Id];
//                        int playerCount = 0;
//                        ClientInventoryItem cItem;
//                        if (G.Game.Inventory.TryGetItem(InventoryObjectType.Material, secondMaterial.Id, out cItem))
//                        {
//                            playerCount += cItem.Count;
//                        }
//                        if (G.Game.Station.StationInventory.TryGetItem(InventoryObjectType.Material, secondMaterial.Id, out cItem))
//                        {
//                            playerCount += cItem.Count;
//                        }
//                        if (playerCount >= needCount)
//                        {
//                            allowedSecond = true;
//                        }
//                    }

//                    if (allowedFirst && allowedSecond)
//                    {
//                        contextActions.Add("Craft", () =>
//                        {
//                            ActionProgress.Setup(1, () =>
//                            {
//                                NRPC.TransformInventoryObjectAndMoveToStationHold(item.Type, item.Id);
//                            });
//                        });
//                    }
//                }

//                contextActions.Add("Destroy", () =>
//                {
//                    NRPC.DestroyInventoryItem(InventoryType.station, item.Type, item.Id);
//                });
//            }
//            else
//            {
//                ClientShipModule item = ((ClientShipModule)e._sender.tag);

//                var info = e._sender.tag as ClientShipModule;
//                if (info.workshop == ((byte)G.Game.UserInfo.GetSelectedCharacter().HomeWorkshop).toEnum<Workshop>())
//                {
//                    contextActions.Add("Equip", () =>
//                    {
//                        NRPC.EquipModuleFromHoldToShip(info.Id);
//                        AudioControloller.Get.PlayUISound(GameSoundsType.module_equip);
//                    });
//                }

//                contextActions.Add("Destroy", () =>
//                {
//                    NRPC.DestroyModule(item.Id);
//                });
//            }

//            //ContextMenuView.Setup(contextActions);

//            //if (e._sender.tag != null)
//            //{
//            //    var info = e._sender.tag as ClientShipModule;
//            //    if (info.workshop == ((byte)G.Game.UserInfo.GetSelectedCharacter().HomeWorkshop).toEnum<Workshop>())
//            //    {
//            //        G.Game.EquipModuleFromHoldToShip(info.Id);
//            //        G.UI.ObjectInfoView.Close();
//            //    }
//            //}
//        }

//        void OnLeftClick(UIEvent e)
//        {

//            Debug.Log("OnLeftClick");
//            //Input.
//        }

//        void OnMouseEnter(UIEvent evt)
//        {
//            showItemInfo = true;
//            manager.StartCoroutine(ShowItemInfo(evt));
//        }

//        void OnMouseExit(UIEvent e)
//        {
//            showItemInfo = false;
//            //SetupItemInfo.HideWhenMouseExitFromSource();
//        }

//        private bool showItemInfo = false;

//        public IEnumerator ShowItemInfo(UIEvent evt)
//        {
//            yield return new WaitForSeconds(0.75f);

            
//            //if (showItemInfo && !ContextMenuView.Visible)
//            //{
//            //    if (evt._sender.tag is IInventoryObjectInfo)
//            //    {
//            //        //SetupItemInfo.ShowWhenMouseEnterOnSource<IInventoryObjectInfo>(evt._sender.tag, SetupInventory.TextureForItem(evt._sender.tag as IInventoryObjectInfo));
//            //    }
//            //    else
//            //    {
//            //        ClientShipModule item = ((ClientShipModule)evt._sender.tag);
//            //        //SetupItemInfo.Setup<ClientShipModule>(item, GetInfoTexture(item));
//            //        //SetupItemInfo.ToMousePosition();
//            //        //SetupItemInfoOnPlayer.Setup<ClientShipModule>(item, GetInfoTexture(item));
//            //        //SetupItemInfoOnPlayer.ToMousePosition();
//            //    }
//            //}
//        }
//    }
//}
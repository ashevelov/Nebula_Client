//namespace Game.Space
//{
//    using UnityEngine;
//    using Photon.Mmo.Client;
//    using Common;
//    using Game.Space;
//    using Game.Space.UI;
//    using System.Collections.Generic;
//    using System.Collections;
//    using Game.Network;
//    using Nebula;

//    /// <summary>
//    /// View for space object selection
//    /// </summary>
//    public class ObjectScreenSelection : ISpaceObjectGUI
//    {
//        private Vector2 _iconAreaSize;

//        //min icon size
//        private Vector2 _minAreaSize = new Vector2(2.0f, 2.0f);
//        private bool _visible;
//        private Texture2D _icon;
//        private ICachedPosition _transform;
//        private bool _initialized;
//        private System.Action _clickAction;
//        private System.Action rbClickAction;

//        private Item _item;

//		private Rect _drawRect;
//		private Rect _drawScaleRect;
//		private float _drawRectScale = 1f;
//		private int _drawRectAnimState = 1;
//        private string _actorName;

//        private float progress = 0;

//        private Texture2D progress_full = null;
//        private Texture2D progress_empty = null;
//        private BaseSpaceObject avatar;

//        private GUIStyle _nameStyle;

//        private List<NotifLabel> massges = new List<NotifLabel>();
//        private List<NotifLabel> removeMassges = new List<NotifLabel>();
//        private Transform _cameraTransform = null;
//        private bool _cameraIsFar = true;
//        private string _color;
//        private Color _iconColor = Color.white;
//        private bool icon = false;

//        private static Color RACE_COLOR_BORGUZANDS = new Color(1, 0.3f, 0.3f, 1);
//        private static Color RACE_COLOR_CRIPTIZOIDS = new Color(0.3f, 1, 0.3f, 1);
//        private static Color RACE_COLOR_HUMANS = new Color(0.3f, 0.3f, 1, 1);

//		private Texture2D iconImg = null;
//        private Texture2D _targetIcon = null;
//        private Texture2D _attitudeIcon = null;
//        private Texture2D _agro_Icon = null;
//		private Texture2D _fractionIcon = null;
//		private GUIStyle _nameFrame = null;
//        private Rect _targetButtonRect;

//        public void SetLeftMouseClickAction(System.Action lbClickAction )
//        {
//            this._clickAction = lbClickAction;
//        }

//        public void SetRightMouseClickAction(System.Action rbClickAction)
//        {
//            this.rbClickAction = rbClickAction;
//        }

//        public void Initialize(ICachedPosition parent, Item item, string iconPath, bool visible, Vector2 iconSize,  System.Action clickAction, System.Action rbClickAction = null) 
//        {
//            _transform = parent;
//            avatar = (BaseSpaceObject)parent;
//			Race race = avatar.Item.Race;

//			if (avatar.Item.Type == (byte)ItemType.Bot)
//			{
//				NpcItem npc = (NpcItem)avatar.Item;
//                if(npc.SubType == BotItemSubType.StandardCombatNpc)
//                {
//                    _actorName = avatar.Item.Name;
//                    _color = "white";
//                    _actorName = _actorName.Color("white");
//                    iconImg = TextureCache.Get(DataResources.Instance.ObjectIcon(Game.Space.Res.IconType.mob).Path);
//                    _attitudeIcon = TextureCache.Get("UI/Textures/Object_icons/netral");
//                    icon = true;
//                }
//                else if(npc.SubType == BotItemSubType.Planet)
//                {
//                    _color = "blue";
//                    iconImg = TextureCache.Get(DataResources.Instance.ObjectIcon(Game.Space.Res.IconType.planet).Path);
//                    _attitudeIcon = TextureCache.Get("UI/Textures/Object_icons/netral");
//                    _actorName = (string.IsNullOrEmpty(avatar.Item.Name) ? "NO NAME" : avatar.Item.Name);
//                    _iconColor = Color.magenta;
//                    icon = true;
//                }
//                else if (npc.SubType == BotItemSubType.StandardCombatNpc)
//                {
                   
//                }
//                else
//                {
//                    _color = "blue";
//                    _actorName = (string.IsNullOrEmpty(avatar.Item.Name) ? "NO NAME" : avatar.Item.Name).Color(_color);
//                    icon = false;
//                }
//            }
//            else if (avatar.Item.Type == (byte)ItemType.Avatar)
//			{
//				//race = ((ProtectionStationItem)avatar.Item).Race;
//				//_actorName = "Player_";
//                //_actorName += avatar.Item.Id.Remove(4);
//                _actorName = avatar.Item.Name;
//                _color = "green";
//                switch (race)
//                { 
//                    case Race.Borguzands:
//                        _iconColor = RACE_COLOR_BORGUZANDS;
//                        _actorName = _actorName.Color("#FF5555");
//                        break;
//                    case Race.Criptizoids:
//                        _iconColor = RACE_COLOR_CRIPTIZOIDS;
//                        _actorName = _actorName.Color("#55FF55");
//                        break;
//                    case Race.Humans:
//                        _iconColor = RACE_COLOR_HUMANS;
//                        _actorName = _actorName.Color("#5555FF");
//                        break;
//                    default:
//                        _iconColor = Color.white;
//                        break;
//                }
//                if (race != MmoEngine.Get.Game.Avatar.Race)
//                {
//                    _attitudeIcon = TextureCache.Get("UI/Textures/Object_icons/enemy");
//                }
//                else
//                {
//                    _attitudeIcon = TextureCache.Get("UI/Textures/Object_icons/netral");
//                }

//                iconImg = TextureCache.Get(DataResources.Instance.ObjectIcon(Game.Space.Res.IconType.player).Path);
//                icon = true;
//            }
//            else
//            {
//                _actorName = avatar.Item.Type.toItemType().ToString().Color("blue");
//                iconImg = TextureCache.Get(DataResources.Instance.ObjectIcon(Game.Space.Res.IconType.station).Path);
//                icon = false;
//            }

//            if (avatar.Item.Type == (byte)ItemType.Asteroid)
//            {
//                iconImg = TextureCache.Get(DataResources.Instance.ObjectIcon(Game.Space.Res.IconType.asteroid).Path);
//            }

//            if (CheckForEventedItem(item))
//            {
//                _iconColor = Color.yellow;
//            }
            

//            if (progress_full == null)
//                progress_full = TextureCache.Get("UI/Textures/green");
//            if (progress_empty == null)
//                progress_empty = TextureCache.Get("UI/Textures/red");
//			if(_targetIcon == null)
//                _targetIcon = TextureCache.Get("UI/Textures/Object_icons/target");
//            if (_agro_Icon == null)
//                _agro_Icon = TextureCache.Get("UI/Textures/Object_icons/agro");
//			if(_fractionIcon == null)
//			{
//				_fractionIcon = TextureCache.Get("UI/Textures/"+race.ToString()+"_logo");
//			}
//            if (_nameFrame == null)
//                _nameFrame = (UnityEngine.Resources.Load("UI/Skins/game") as GUISkin).box;

//            _icon = TextureCache.Get(iconPath);
//            _visible = visible;
//            _iconAreaSize = iconSize;
//            _clickAction = clickAction;
//            this.rbClickAction = rbClickAction;
//            _item = item;
//            _initialized = true;
//            InitStyle();
//            if (UIManager.Get)
//                UIManager.Get.AddSpaceObjectGui(this);
//        }


//        private void InitStyle() {
//            _nameStyle = UIManager.Get.GetSkin("game").GetStyle("font_middle_center2");
//        }
//        public void Release() {
//            if (UIManager.Get)
//                UIManager.Get.RemoveSpaceObjectGui(this);
//        }

//        public void SetVisible(bool visible) {
//            _visible = visible;
//        }

//        public void Update()
//        {
//            if (false == NetworkGame.OperationsHelper.GuiHided)
//            {

//                if (_initialized && avatar && avatar.gameObject && avatar.gameObject.activeSelf)
//                {
//                    if (_visible)
//                    {
//                        if (_cameraTransform == null)
//                        {

//                            _cameraTransform = MouseOrbitRotateZoom.Get.transform; //Camera.main.transform;
//                        }
//                        else
//                        {
//                            if (Camera.main)
//                                _cameraIsFar = Vector3.Distance(Camera.main.transform.position, avatar.transform.position) > 30000;
//                            else
//                                _cameraIsFar = true;
//                        }
//                        Vector2 currentSize = _iconAreaSize;
//                        if (avatar && MmoEngine.Get.Game.Avatar.View)
//                        {
//                            var otherPos = avatar.Item.GetPosition();
//                            var playerPos = MmoEngine.Get.Game.Avatar.View.transform.position;
//                            float dist = Vector3.Distance(otherPos, playerPos);
//                            dist = Mathf.Clamp(dist, 0, 10000);
//                            float t = 1.0f - Mathf.InverseLerp(0, 10000, dist);
//                            currentSize = Vector2.Lerp(_minAreaSize, _iconAreaSize, 1);
//                            //Debug.Log("t: " + t);
//                        }
//						_drawRectScale = Mathf.Clamp(_drawRectScale + (Time.deltaTime * _drawRectAnimState), 0.8f, 1.2f);
//						_drawRectAnimState = (_drawRectScale <= 0.8f || _drawRectScale >= 1.2f) ? -_drawRectAnimState : _drawRectAnimState;
//                        _drawRect = Utils.WorldPos2ScreenRect(_transform.Position, currentSize/2);
//						_drawScaleRect = Utils.WorldPos2ScreenRect(_transform.Position, currentSize * _drawRectScale);
//                        Rect touchRect = Utils.WorldPos2ScreenRect(_transform.Position, _iconAreaSize);
//                        if (Input.GetMouseButtonDown(0))
//                        {
//                            if (touchRect.Contains(Utils.ConvertedMousePosition))
//                            {
//                                if (_clickAction != null)
//                                    _clickAction();
//                            }
//                        }
//                        if(Input.GetMouseButtonDown(1))
//                        {
//                            if(touchRect.Contains(Utils.ConvertedMousePosition))
//                            {
//                                if (this.rbClickAction != null)
//                                    this.rbClickAction();
//                            }
//                        }

//                        /*
//                        if (touchRect.Contains(Utils.ConvertedMousePosition))
//                        {
//                            if (_item != null)
//                            {
//                                if (_item is IObjectInfo)
//                                {
//                                    G.UI.TargetInfoView.SetObject((IObjectInfo)_item);
//                                }
//                            }
//                        }*/

//                        massges.ForEach((m) =>
//                        {
//                            m.style.normal.textColor = m.style.normal.textColor * (new Color(1, 1, 1, 1 - (Time.deltaTime * 0.5f)));
//                            m.pos -= new Vector2(0, 80 * Time.deltaTime);

//                            if (m.style.normal.textColor.a <= 0)
//                            {
//                                removeMassges.Add(m);
//                            }
//                        });

//                        removeMassges.ForEach((rm) =>
//                        {
//                            massges.Remove(rm);
//                        });
//                        removeMassges.Clear();
//                    }
//                }
//            }
//        }

//        private float time;

//        public void AddMassage(string text, Color color)
//        {
//            NotifLabel temp = new NotifLabel();
//            temp.Setup(text, color);
//            temp.rect = new Rect(0, 0, 0, 0);
//            temp.pos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
//            massges.Add(temp);
//        }

//        public void Draw() 
//        {
//            if (false == NetworkGame.OperationsHelper.GuiHided)
//            {
//                if (_visible && !MapController.MapExist() && !_cameraIsFar)
//                {

//                    if (Event.current.type == EventType.Repaint)
//                    {
//                        //don't draw chest gui when invisible
//                        if(_item.Type == ItemType.Chest.toByte())
//                        {
//                            if (_item.View) 
//                            {
//                                if (!_item.Component.ChildrensActive)
//                                    return;
//                            }
//                        }

//                        Color oldColor = GUI.color;

//                        Rect nameRct = _drawRect.addPos(new Vector2(0, -50));
//                        Rect rct = nameRct;


//                        if (MmoEngine.Get.Game.Avatar != null)
//                        {
//                            if (MmoEngine.Get.Game.Avatar.Target.HasTarget && _item.Id == MmoEngine.Get.Game.Avatar.Target.TargetId)
//                            {
//                                /*
//                                GUI.BeginGroup(rct.addSize(new Vector2(200, 20)), GUI.skin.box);
//                                if (avatar.Item is IDamagable)
//                                {
//                                    progress = ((IDamagable)avatar.Item).GetHealth01();

//                                    Rect hp = new Rect(0, 0, 160, 4);
//                                    GUI.Label(hp.addPos(80, 10), _actorName, _nameStyle);
//                                    hp = hp.addPos(new Vector2(80, 25));
//                                    Hashtable targetInfo = MmoEngine.Get.Game.Avatar.Target.GetTargetInfo();
//                                    string dist = string.Empty;
//                                    if (targetInfo.ContainsKey("distance"))
//                                    {
//                                        dist = string.Format("{0:F1}", (float)targetInfo["distance"]);
//                                    }
//                                    Rect iconRct = new Rect(0, 0, 70, 70);
//                                    GUI.Label(hp.addPos(0, 25), "Distance: " + dist, _nameStyle);
//                                    if (iconImg != null)
//                                    {
//                                        GUI.DrawTexture(iconRct, iconImg);
//                                    }
//                                    GUI.DrawTexture(hp, progress_empty);
//                                    hp.width = hp.width * progress;
//                                    GUI.DrawTexture(hp, progress_full);
//                                }

//                                GUI.EndGroup();
//                                */
//								GUI.color = Color.yellow;
//								if(_targetIcon != null)
//								{
//									GUI.DrawTexture(_drawScaleRect, _targetIcon);
//								}

//                                GUI.color = oldColor;

//						        if(icon)
//                                {
//							        nameRct = nameRct.addSize(new Vector2(50, 0));
//							        nameRct.x = _drawRect.center.x - nameRct.width * 0.5f;
//							        if (_nameStyle == null)
//								        InitStyle();
//							        Rect fracIconRect = new Rect(nameRct.x, nameRct.y, 30,30);
//                                    GUI.Box(nameRct.addSize(new Vector2(60, 10)).addPos(-3,-3), GUIContent.none , _nameFrame);
//							        GUI.DrawTexture(fracIconRect, _fractionIcon);
//							        GUI.Label(nameRct.addPos(40,0), _actorName);
//                                    _targetButtonRect = fracIconRect.addSize(new Vector2( nameRct.width, 0));

//                                    if (avatar.Item is IDamagable)
//                                    {
//                                        progress = ((IDamagable)avatar.Item).GetHealth01();

//                                        Rect hp = new Rect(40, 23, 85, 4).addPos(nameRct);
//                                        GUI.DrawTexture(hp, progress_empty);
//                                        hp.width = hp.width * progress;
//                                        GUI.DrawTexture(hp, progress_full);
//                                    }
//						        }
//                            }
//                        }
//                        GUI.color = _iconColor;
//                        //GUI.color = CheckForItemTarget(_item) ? Color.red : GUI.color;
//                        if (iconImg != null)
//                        {
//                            GUI.DrawTexture(_drawRect.addPos(3,3).addSize(-6, -6), iconImg);
//                        }

//                        GUI.color = oldColor;
//                        if (CheckForItemTarget(_item) && _attitudeIcon != null)
//                        {
//                            GUI.DrawTexture(_drawRect.addPos(-3, -3).addSize(6, 6), _agro_Icon);
//                        }
//                        if (_attitudeIcon != null)
//                        {
//                            GUI.DrawTexture(_drawRect, _attitudeIcon);
//                        }

////                        if (G.Game.Avatar != null)
////                        {
////                            if (this._item.Id != G.Game.Avatar.Target.TargetId)
////                            {
////                                if (icon)
////                                    GUI.Label(_drawRect, "+".Size(25).Color(_color), _nameStyle);
////                            }
////                        }
                        
//                        massges.ForEach((m) =>
//                        {
//                            m.rect = nameRct.addPos(0, 20).addPos(m.pos);
//                            GUI.color = m.color;
//                            m.Draw();
//                        });

						
//                    }
//                    if (GUI.Button(_targetButtonRect, GUIContent.none, GUIStyle.none))
//                    {
//                        //        if (G.Game.Avatar != null && G.Game.Avatar.Target.HasTarget)
//                        {
//                            G.Game.Avatar.RequestTarget(avatar.Item.Id, avatar.Item.Type, true);
//                            Debug.Log("avatar.Item.Id" + avatar.Item.Id);
//                        }
//                    }
//                }
//            }
//        }


//        public GameObject GetObject()
//        {
//            return _transform.GetObject();
//        }

//        public bool CheckForEventedItem(Item item)
//        {
//            if (!item.IsMine)
//            {
//                if (item.Type.toItemType() == ItemType.Bot)
//                {
//                    NpcItem npcItem = item as NpcItem;
//                    if (npcItem.SubType == BotItemSubType.StandardCombatNpc)
//                    {
//                        StandardNpcCombatItem combatItem = npcItem as StandardNpcCombatItem;
//                        if (combatItem.EventInfo.FromEvent)
//                            return true;
//                    }
//                }
//            }
//            return false;
//        }

//        public bool CheckForItemTarget(Item item)
//        {
//            if (!item.IsMine)
//            {
//                if (item.Type.toItemType().AnyFrom<ItemType>(ItemType.Bot, ItemType.Avatar))
//                {
//                    ForeignItem foreignItem = item as ForeignItem;

//                    if (foreignItem.TargetInfo.HasTarget)
//                    {
//                        if (foreignItem.TargetInfo.TargetId == G.Game.Avatar.Id)
//                        {
//                            return true;
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//    }
//}

/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Photon.Mmo.Client;
using Game.Space.Res;
using Game.Space;
using Nebula.Client.Inventory;
using Nebula.Client;

namespace Game.Space.UI
{
    public static class SetupShipPanel
    {
        public static void Setup(MonoBehaviour instance)
        {
            UIResizeableGroup group = UIManager.Get.GetLayout("ship_panel") as UIResizeableGroup;
            UIButton closeButton = group.GetChildrenEntityByName("close_button") as UIButton;

            UIManager.Get.RegisterEventHandler("SHIP_PANEL_CLOSE_CLICK", null, (evt) =>
            {
                if (group._visible)
                {
                    group.SetVisibility(false);
                }
            });


            UIScrollView scrollView = group.GetChildrenEntityByName("info_panel") as UIScrollView;

            scrollView.SetUpdateInterval(1f);
            scrollView.RegisterUpdate((parent) =>
            {
                UpdateCharacteristics(group);
            });


            UIManager.Get.RegisterEventHandler("SHIP_PANEL_SHOW_INFO_TOGGLE", null, (evt) =>
            {
                switch (evt._sender.tag.ToString())
                {
                    case "energeShow":
                        _energeShow = !_energeShow;
                        break;
                    case "healthShow":
                        _healthShow = !_healthShow;
                        break;
                    case "resistShow":
                        _resistShow = !_resistShow;
                        break;
                    case "speedShow":
                        _speedShow = !_speedShow;
                        break;
                    case "weaponShow":
                        _weaponShow = !_weaponShow;
                        break;
                    case "skillsShow":
                        _skillsShow = !_skillsShow;
                        break;
                    case "inventoryShow":
                        _inventoryShow = !_inventoryShow;
                        break;
                }
            });

            UIGroup ship_view = group.GetChildrenEntityByName("ship_view") as UIGroup;

            //group.RegisterUpdate((parent) =>
            //{
            //    ImageUpdate(ship_view, instance);
            //});

            instance.StartCoroutine(ImageUpdate(ship_view, instance));


            //group.SetWillVisibleAction("SHIP_PANEL_UPDATE_MODULES", () => UpdateModules());
            UpdateModelesButton();

        }

        public static void UpdateModelesButton()
        {
            Debug.Log("UpdateModelesButton");
            UIResizeableGroup group = UIManager.Get.GetLayout("ship_panel") as UIResizeableGroup;
            UIGroup ship_view = group.GetChildrenEntityByName("ship_view") as UIGroup;

            ship_view.AddChild(AddModuleButton(ship_view, "cb_"));
            ship_view.AddChild(AddModuleButton(ship_view, "df_"));
            ship_view.AddChild(AddModuleButton(ship_view, "dm_"));
            ship_view.AddChild(AddModuleButton(ship_view, "cm_"));
            ship_view.AddChild(AddModuleButton(ship_view, "es_"));
            ship_view.AddChild(AddModuleButton(ship_view, "weapon_"));
        }

        private static UIButton AddModuleButton(UIGroup ship_view, string id)
        {
            UITexture cb_icon = ship_view.GetChildrenEntityByName(id+"icon") as UITexture;

            UIButton cb_btn = new UIButton { _enabled = true, _name = id + "btn", _parent = ship_view, _text = string.Empty, _style = UIManager.Get.GetSkin("game").GetStyle("cell_button"), _visible = true, align = UIAlign.None, blockMouse = true, selfDrawable = false, zorder = 0, tag = null };

            cb_btn.SetRect(cb_icon._rect, UIUnits.px);
            cb_btn.events = new List<UIEvent>
                {
                    new UIEvent { _name = "SHIP_PANEL_CELL_ON_MOUSE_ENTER_EVENT", _parameters = new Hashtable(), _sender = cb_btn, EventType = UIEventType.OnEnter },
                    new UIEvent { _name = "SHIP_PANEL_CELL_ON_MOUSE_EXIT_EVENT", _parameters = new Hashtable(), _sender = cb_btn, EventType = UIEventType.OnExit }
                };
            cb_btn.RegisterHandler(UIEventType.OnEnter, OnMouseEnter);
            cb_btn.RegisterHandler(UIEventType.OnExit, OnMouseExit);
            return cb_btn;
        }

        public static void UpdateModules()
        {
            Debug.Log("UpdateModules");
            UIResizeableGroup group = UIManager.Get.GetLayout("ship_panel") as UIResizeableGroup;
            UIGroup ship_view = group.GetChildrenEntityByName("ship_view") as UIGroup;

            UIButton cb_btn = group.GetChildrenEntityByName("cb_btn") as UIButton;
            //Game.Space.ClientShipModule info = G.Game.Ship.ShipModel.Module(ShipModelSlotType.CB);
            cb_btn.tag = G.Game.Ship.ShipModel.Module(ShipModelSlotType.CB);
        }

        private static Camera _camera;
        private static Camera _mainCamera;

        private static UITexture image;
        private static Texture2D captured;
        private static IEnumerator ImageUpdate(UIEntity entity, MonoBehaviour instance)
        {
            if (entity._parent._visible)
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                if (_camera == null)
                {
                    GameObject go = GameObject.Instantiate(PrefabCache.Get("Prefabs/Ship_info_image")) as GameObject;
                    go.transform.position = Vector3.zero;
                    _camera = go.GetComponentInChildren<Camera>();
                }
                else
                {
                    if (image == null)
                        image = (entity as UIGroup).GetChildrenEntityByName("ship_image") as UITexture;

                    if(captured == null)
                        captured = new Texture2D((int)image._rect.width, (int)image._rect.height);
                    RenderTexture.active = _camera.targetTexture;
                    captured.ReadPixels(new Rect(0, 0, image._rect.width, image._rect.width), 0, 0);
                    captured.Apply();
                    RenderTexture.active = null;
                    yield return new WaitForEndOfFrame();
                    image._texture = captured;
                    //UnityEngine.Resources.UnloadUnusedAssets();
                }
                //yield return new WaitForSeconds(0.3f);
                instance.StartCoroutine(ImageUpdate(entity, instance));
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                instance.StartCoroutine(ImageUpdate(entity, instance));
            }


        }

        private static float scrollHeight = 0;

        private static void UpdateCharacteristics(UIEntity entity)
        {

            UIScrollView scrollView = entity.GetChildrenEntityByName("info_panel") as UIScrollView;
            UIGroup back = entity.GetChildrenEntityByName("info_panle") as UIGroup;
            back._rect.height = entity._rect.height - 30;

            UIGroup ship_view = entity.GetChildrenEntityByName("ship_view") as UIGroup;

            UIButton cb_btn = ship_view.GetChildrenEntityByName("cb_btn") as UIButton;
            cb_btn.tag = G.Game.Ship.ShipModel.Module(ShipModelSlotType.CB);

            UIButton df_btn = ship_view.GetChildrenEntityByName("df_btn") as UIButton;
            df_btn.tag = G.Game.Ship.ShipModel.Module(ShipModelSlotType.DF);

            UIButton dm_btn = ship_view.GetChildrenEntityByName("dm_btn") as UIButton;
            dm_btn.tag = G.Game.Ship.ShipModel.Module(ShipModelSlotType.DM);

            UIButton cm_btn = ship_view.GetChildrenEntityByName("cm_btn") as UIButton;
            cm_btn.tag = G.Game.Ship.ShipModel.Module(ShipModelSlotType.CM);

            UIButton es_btn = ship_view.GetChildrenEntityByName("es_btn") as UIButton;
            es_btn.tag = G.Game.Ship.ShipModel.Module(ShipModelSlotType.ES);

            UIButton weapon_btn = ship_view.GetChildrenEntityByName("weapon_btn") as UIButton;
            weapon_btn.tag = G.Game.Ship.Weapon.WeaponObject;


            scrollView.Clear();
            scrollHeight = 0;

            var template = scrollView.CreateElement();
            
            scrollView.AddChild(Enegry(template));
            scrollView.AddChild(Health(template));
            scrollView.AddChild(Resist(template));
            scrollView.AddChild(Speed(template));
            scrollView.AddChild(Weapon(template));
            scrollView.AddChild(Skills(template));
            scrollView.AddChild(Inventory(template));

            //MmoEngine.Get.Game.Avatar.SetNewRandomSlotModule (Common.ShipModelSlotType.ES);

            //MmoEngine.Get.Game.Avatar.SetNewRandomSlotModule(Common.ShipModelSlotType.ES);

        }


        private static bool _energeShow = true;
        private static UIScrollItemTemplate Enegry( UIScrollItemTemplate panel)
        {

            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;
            toggle.tag = "energeShow";
            toggle._value = _energeShow;

            //name = template.GetChildrenEntityByName("name_panel") as UILabel;
            //name._text = "Ship info";

            scrollView.Clear();

            toggle._text = "Enegry";
            if (_energeShow)
            {
                var game = G.Game;

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                nameText._text = "Max Energy: ";
                paramLabel._text = game.CombatStats.MaxEnergy.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);


                nameText._text = "Current Energy: ";
                paramLabel._text =game.CombatStats.CurrentEnergy.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);


                nameText._text = "Energy Restoration: ";
                paramLabel._text = game.CombatStats.EnergyRestoration.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

            }

            return OffsetPanel(panel);
        }

        private static bool _healthShow = true;
        private static UIScrollItemTemplate Health(UIScrollItemTemplate panel)
        {

            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;
            toggle.tag = "healthShow";
            toggle._value = _healthShow;

            //name = template.GetChildrenEntityByName("name_panel") as UILabel;
            //name._text = "Ship info";

            scrollView.Clear();

            toggle._text = "Health";
            if (_healthShow)
            {
                var game = G.Game;

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                nameText._text = "Max HP: ";
                paramLabel._text =game.CombatStats.MaxHP.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Current HP: ";
                paramLabel._text = game.CombatStats.CurrentHP.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Restoration HP Per Sec: ";
                paramLabel._text = game.CombatStats.RestorationHPPerSec.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Restoration HP Per Shot: ";
                paramLabel._text = game.CombatStats.RestorationHPPerShot.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

            }

            return OffsetPanel(panel);
        }

        private static bool _resistShow = true;
        private static UIScrollItemTemplate Resist(UIScrollItemTemplate panel)
        {

            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;
            toggle.tag = "resistShow";
            toggle._value = _resistShow;

            //name = template.GetChildrenEntityByName("name_panel") as UILabel;
            //name._text = "Ship info";

            scrollView.Clear();

            toggle._text = "Resists";
            if (_resistShow)
            {
                var game = G.Game;

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                nameText._text = "Damage Resist: ";
                paramLabel._text = game.CombatStats.DamageResist.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Damage Resist Blocked: ";
                paramLabel._text = (game.CombatStats.DamageResistBlocked ? "YES" : "NO");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);
            }

            return OffsetPanel(panel);
        }

        private static bool _speedShow = true;
        private static UIScrollItemTemplate Speed(UIScrollItemTemplate panel)
        {

            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;
            toggle.tag = "speedShow";
            toggle._value = _speedShow;

            //name = template.GetChildrenEntityByName("name_panel") as UILabel;
            //name._text = "Ship info";

            scrollView.Clear();

            toggle._text = "Speed";
            if (_speedShow)
            {
                var game = G.Game;

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                nameText._text = "Max Speed: ";
                paramLabel._text = game.CombatStats.MaxSpeed.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Current Speed: ";
                paramLabel._text = game.CombatStats.CurrentSpeed.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);
            }

            return OffsetPanel(panel);
        }

        private static bool _weaponShow = true;
        private static UIScrollItemTemplate Weapon(UIScrollItemTemplate panel)
        {

            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;
            toggle.tag = "weaponShow";
            toggle._value = _weaponShow;

            scrollView.Clear();

            toggle._text = "Weapon";
            if (_weaponShow)
            {
                var game = G.Game;

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                nameText._text = "Weapon Optimal Distance: ";
                paramLabel._text = game.CombatStats.WeaponOptimalDistance.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weapon Cooldown: ";
                paramLabel._text = game.CombatStats.WeaponCooldown.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Heavy Weapon Damage: ";
                paramLabel._text = game.CombatStats.HeavyWeaponDamage.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Light Weapon Damage: ";
                paramLabel._text = game.CombatStats.LightWeaponDamage.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weapon Precision: ";
                paramLabel._text = game.CombatStats.WeaponPrecision.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weapon Damage On Area: ";
                paramLabel._text = game.CombatStats.WeaponDamageOnArea.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weapon Critical Chance: ";
                paramLabel._text = game.CombatStats.WeaponCriticalChance.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Heavy Weapon Critical Damage: ";
                paramLabel._text = game.CombatStats.WeaponHeavyCriticalDamage.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weapon Blocked: ";
                paramLabel._text = game.CombatStats.WeaponBlocked ? "YES" : "NO";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);
            }

            return OffsetPanel(panel);
        }

        private static bool _skillsShow = true;
        private static UIScrollItemTemplate Skills(UIScrollItemTemplate panel)
        {

            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;
            toggle.tag = "skillsShow";
            toggle._value = _skillsShow;

            scrollView.Clear();

            toggle._text = "Skills";
            if (_skillsShow)
            {
                var game = G.Game;

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                nameText._text = "Skill Block Prob: ";
                paramLabel._text = game.CombatStats.SkillBlockProb.ToString("F1");
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);
            }

            return OffsetPanel(panel);
        }

        private static bool _inventoryShow = true;
        private static UIScrollItemTemplate Inventory(UIScrollItemTemplate panel)
        {

            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;
            toggle.tag = "inventoryShow";
            toggle._value = _inventoryShow;

            scrollView.Clear();

            toggle._text = "Inventory";
            if (_inventoryShow)
            {
                var game = G.Game;

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                nameText._text = "Inventory Slots Count: ";
                paramLabel._text = game.CombatStats.InventorySlotsCount.ToString();
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Inventory Slots Used: ";
                paramLabel._text = game.CombatStats.InventorySlotsUsed.ToString();
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);
            }

            return OffsetPanel(panel);
        }


        private static UIScrollItemTemplate OffsetPanel(UIScrollItemTemplate panel)
        {
            UIScrollView scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;
            UIToggle toggle = panel.GetChildrenEntityByName("show_info_toggle") as UIToggle;

            scrollView._rect.height = scrollView._viewRect.height + 22;
            panel._rect.height = scrollView._rect.height;

            UIScrollItemTemplate itemTemplate = panel.Copy as UIScrollItemTemplate;
            itemTemplate.useComputedHeight = true;
            itemTemplate.SetComputeHeight(size =>
            {
                return itemTemplate._rect.height + 5;
            });

            return itemTemplate;
        }
        private static void OnMouseEnter(UIEvent evt)
        {
            Debug.Log("OnMouseEnter");
            if (evt._sender.tag is IStationHoldableObject)
            {
                //SetupItemInfo.Setup<ClientShipModule>(evt._sender.tag, TextureForItem(evt._sender.tag as IInfo));
            }
            if (evt._sender.tag is IInventoryObjectInfo)
            {
                //SetupItemInfo.Setup<IInventoryObjectInfo>(evt._sender.tag, TextureForItem(evt._sender.tag as IInfo));
            }
            //SetupItemInfo.ToMousePosition();
        }

        private static Texture2D TextureForItem(IInfo info)
        {
            if (info is IStationHoldableObject)
            {
                ClientShipModule csModele = info as ClientShipModule;
                string id = csModele.prefab.Remove(0, 22);

                return TextureCache.Get("UI/Textures/Modules/" + id);
            }
            else if (info is IInventoryObjectInfo)
            {
                return TextureCache.Get("UI/Textures/weapon");
            }
            else
            {
                return TextureCache.Get("UI/Textures/transparent");
            }
        }

        private static void OnMouseExit(UIEvent e)
        {
            //SetupItemInfo.Hide();
            //SetupItemInfoOnPlayer.Hide();
        }
    }
}
*/
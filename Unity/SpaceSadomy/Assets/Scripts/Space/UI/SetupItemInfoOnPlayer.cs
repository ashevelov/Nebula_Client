/*
namespace Game.Space.UI {
    using UnityEngine;
    using Photon.Mmo.Client;
    using Game.Space.Resources;
    using System.Collections;
    using System.Collections.Generic;
    using Common;
    using Nebula.Client.Inventory;
    using Nebula.Client.Inventory.Objects;
    using Nebula;
    using Nebula.Client;

    public static class SetupItemInfoOnPlayer
    {
        private static UIEntity rootPanel;

        public static bool Visible
        {
            get
            {
                if (rootPanel != null)
                    return rootPanel.Visible;
                else
                    return false;
            }
        }

        public static void Setup<T>(object objInfo, Texture2D iconTex)
        {
            UIEntity panel = UIManager.Get.GetLayout("item_info_on_player");
            rootPanel = panel;

            panel.SetVisibility(true);

            var scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;

            var world = MmoEngine.Get.Game.World;


            var name = panel.GetChildrenEntityByName("name") as UILabel;
            var onPlayer = panel.GetChildrenEntityByName("on_player") as UILabel;
            onPlayer.SetText("installed on the ship");
            //var icon = panel.GetChildrenEntityByName("icon") as UITexture;


            UIManager.Get.RegisterEventHandler("ITEM_INFO_CLOSE_CLICK", null, (evt) =>
            {
                panel.SetVisibility(false);
            });

		
			var skillName = panel.GetChildrenEntityByName("skill_name") as UILabel;
            var skilldesc = panel.GetChildrenEntityByName("skill_description") as UILabel;
            var glass = panel.GetChildrenEntityByName("glass") as UITexture;


			skillName.SetText("");
			skilldesc.SetText("");

            //scrollView.RegisterUpdate((e) =>
            //{
            //    //scrollView._viewPosition.y += Input.GetAxis("");
            //    //scrollView.SetScrollPosition(new Vector2(0, ));
            //    //chatScrollView.SetScrollPosition(new Vector2(0, float.MaxValue));
            //});




            scrollView.Clear();

            if (typeof(T) == typeof(IInventoryObjectInfo))
            {
                IInventoryObjectInfo info = (IInventoryObjectInfo)objInfo;

                //icon.SetTexture(iconTex);

                switch (info.Type)
                {
                    case Common.InventoryObjectType.Weapon:
                        {

                            ClientPlayerShipWeapon playerWeapon = G.Game.Ship.Weapon;
                            var template = scrollView.CreateElement();
                            var nameText = template.GetChildrenEntityByName("name") as UILabel;
                            var paramLabel = template.GetChildrenEntityByName("param") as UILabel;
                            
                            scrollView._rect.width = 250;
                            template._rect.width = 250;
                            nameText._rect.width = 240;
                            paramLabel._rect.width = 240;
                            panel._rect.width = 250;
                            panel._rect.height = 370;
                            name._rect.width = 250;
                            onPlayer._rect.width = 250;
                            glass._rect.width = 250;
                            glass._rect.height = 370;

                            WeaponInventoryObjectInfo wInfo = info as WeaponInventoryObjectInfo;

                            name._text = "Weapon";
                            
                            nameText._text = "Heavy Damage: ";
                            paramLabel._text = (int)playerWeapon.HeavyDamage + "";
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Heavy Cooldown: ";
                            paramLabel._text = (int)playerWeapon.HeavyCooldown + "";
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Distance: ";
                            paramLabel._text = (int)playerWeapon.OptimalDistance + "";
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                        }
                        break;

                }

            }
            else if (typeof(T).Name == typeof(ClientShipModule).Name)
            {
                ClientShipModule playerModule = (ClientShipModule)objInfo;
                ClientShipModule info = G.Game.Ship.ShipModel.Module(playerModule.type);

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;


                scrollView._rect.width = 450;
                template._rect.width = 450;
                nameText._rect.width = 440;
                paramLabel._rect.width = 440;
                panel._rect.width = 450;
                panel._rect.height = 670;
                name._rect.width = 450;
                onPlayer._rect.width = 450;
                glass._rect.width = 450;
                glass._rect.height = 670;

                name._style = new GUIStyle(name._style);
                name._style.normal.textColor = Utils.GetColor(info.color);
                name._text = "Module";

                nameText._text = "Name: ";
                paramLabel._text = info.name;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weapon slots count: ";
                paramLabel._text = info.weaponSlotsCount + "";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Health points : ";
                paramLabel._text = ((int)info.hp) + "" ;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Hold: ";
                paramLabel._text = (int)info.hold + "";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Resists: ";
                paramLabel._text = (int)info.resist + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Speed: ";
                paramLabel._text = ((int)info.Speed) + "";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Damage bonus: ";
                paramLabel._text = (int)info.damageBonus + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Cooldown bonus: ";
                paramLabel._text = (int)info.cooldownBonus + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Distance bonus: ";
                paramLabel._text = (int)info.distanceBonus + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weight: ";
                paramLabel._text = ((int)info.weight) + "";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

				if(info.HasSkill)
				{	
					var skillData = DataResources.Instance.SkillData(info.skill);
					if (skillData != null)
					{
						skillName.SetText(skillData.name);
						skilldesc.SetText(skillData.description.FormatBraces());
						
					}
					
				}

            }
            else
            {
            }
           
        }
        private static Color GetColorText(float carentValuer, float newItemValue)
        {
            float delta = newItemValue - carentValuer;
            Color color;
            if (delta > 0)
                color = Color.green;
            else if (delta == 0)
                color = Color.yellow;
            else
                color = Color.red;

            return color;
        }

        private static string GetFullText(float carentValuer, float newItemValue)
        {
            float delta = newItemValue - carentValuer;
            string fullText;
            if (delta > 0)
                fullText = (int)newItemValue + "  -+" + (int)delta;
            else if (delta == 0)
                fullText = ((int)newItemValue).ToString();
            else
                fullText = (int)newItemValue + "  -" + (int)delta;

            return fullText;
        }

        public static void Hide()
        {
            UIEntity panel = UIManager.Get.GetLayout("item_info_on_player");
            panel._visible = false;
        }

        public static void ToMousePosition()
        {
            UIEntity panel_target = UIManager.Get.GetLayout("item_info");
            UIEntity panel = UIManager.Get.GetLayout("item_info_on_player");
            Vector3 pos = Utils.ConvertedMousePosition;
            if (pos.x > Screen.width / 2 / Utils.GameMatrix().m00)
            {
                panel._rect.x = pos.x - panel._rect.width - panel_target._rect.width;
            }
            else
            {
                panel._rect.x = pos.x + panel_target._rect.width;
            }

            if (pos.y < ( Screen.height -( Screen.height / 3))/Utils.GameMatrix().m00)
            {
                panel._rect.y = pos.y;
            }
            else
            {
                panel._rect.y = pos.y - panel._rect.height;
            }
        }
        
    }
}
*/

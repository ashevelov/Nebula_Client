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

    public static class SetupItemInfo
    {

        public static void Setup<T>(object objInfo, Texture2D iconTex)
        {
            UIEntity panel = UIManager.Get.GetLayout("item_info");
            panel.SetVisibility(true);

            var scrollView = panel.GetChildrenEntityByName("info") as UIScrollView;

            var world = MmoEngine.Get.Game.World;


            var name = panel.GetChildrenEntityByName("name") as UILabel;
            var icon = panel.GetChildrenEntityByName("icon") as UITexture;



			var skillText = panel.GetChildrenEntityByName("skill") as UILabel;			
			var skillName = panel.GetChildrenEntityByName("skill_name") as UILabel;			
			var skillicon = panel.GetChildrenEntityByName("skill_icon") as UITexture;
            var skilldesc = panel.GetChildrenEntityByName("skill_description") as UILabel;
            var glass = panel.GetChildrenEntityByName("glass") as UITexture;
			
			skillText.SetText("");
			skillicon._texture =  TextureCache.Get("UI/Textures/transparent");
			skillName.SetText("");
			skilldesc.SetText("");


            UIManager.Get.RegisterEventHandler("ITEM_INFO_CLOSE_CLICK", null, (evt) =>
            {
                panel.SetVisibility(false);
            });




            //scrollView.RegisterUpdate((e) =>
            //{
            //    //scrollView._viewPosition.y += Input.GetAxis("");
            //    //scrollView.SetScrollPosition(new Vector2(0, ));
            //    //chatScrollView.SetScrollPosition(new Vector2(0, float.MaxValue));
            //});

			icon.SetTexture(iconTex);


            scrollView.Clear();

            if (typeof(T) == typeof(IInventoryObjectInfo))
            {
                IInventoryObjectInfo info = (IInventoryObjectInfo)objInfo;

                

                switch (info.Type)
                {
                    case Common.InventoryObjectType.Weapon:
                        {

                           
                            ClientPlayerShipWeapon playerWeapon = G.Game.Ship.Weapon;
                            var template = scrollView.CreateElement();
                            var nameText = template.GetChildrenEntityByName("name") as UILabel;
                            var paramLabel = template.GetChildrenEntityByName("param") as UILabel;


                            scrollView._rect.width = 300;
                            template._rect.width = 300;
                            nameText._rect.width = 290;
                            paramLabel._rect.width = 290;
                            panel._rect.width = 300;
                            panel._rect.height = 400;
                            name._rect.width = 300;
                            glass._rect.width = 300;
                            glass._rect.height = 400;

                            WeaponInventoryObjectInfo wInfo = info as WeaponInventoryObjectInfo;

                            name._text = "Weapon";
                            
                            int delta;

                            nameText._text = "Heavy Damage: ";
                            delta = (int)(wInfo.HeavyDamage - playerWeapon.WeaponObject.HeavyDamage);
                            paramLabel.SetTextColor(delta <0 ? Color.red : Color.green);
                            paramLabel._text = (int)wInfo.HeavyDamage + "    " + (delta <0 ? "" : "+") +  "" + delta;
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Heavy energy: ";
                            delta = (int)(wInfo.HeavyEnergy - playerWeapon.WeaponObject.HeavyEnergy);
                            paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                            paramLabel._text = (int)wInfo.HeavyEnergy + "    " + (delta < 0 ? "" : "+") + "" + delta;
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Heavy Cooldown: ";
                            delta = (int)(wInfo.HeavyCooldown - playerWeapon.WeaponObject.HeavyCooldown);
                            paramLabel.SetTextColor(delta > 0 ? Color.red : Color.green);
                            paramLabel._text = (int)wInfo.HeavyCooldown + "    " + (delta < 0 ? "" : "+") + "" + delta;
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Light Damage: ";
                            delta = (int)(wInfo.LightDamage - playerWeapon.WeaponObject.LightDamage);
                            paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                            paramLabel._text = (int)wInfo.LightDamage + "    " + (delta < 0 ? "" : "+") + "" + delta;
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = " LightCooldown: ";
                            delta = (int)(wInfo.LightCooldown - playerWeapon.WeaponObject.LightCooldown);
                            paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                            paramLabel._text = (int)wInfo.LightCooldown + "    " + (delta < 0 ? "" : "+") + "" + delta;
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Distance: ";
                            delta = (int)(wInfo.OptimalDistance - playerWeapon.WeaponObject.OptimalDistance);
                            paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                            paramLabel._text = (int)wInfo.OptimalDistance + "    " + (delta < 0 ? "" : "+") + "" + delta;
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Level: ";
                            paramLabel.SetTextColor(Color.white);
                            paramLabel._text = wInfo.Level.ToString();
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Workshop: ";
                            paramLabel.SetTextColor(Color.white);
                            var weaponTemplate = DataResources.Instance.Weapon(wInfo.Template);
                            paramLabel._text = weaponTemplate.Workshop.ToString();
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                        }
                        break;
                    case Common.InventoryObjectType.Scheme:
                        {
                            var template = scrollView.CreateElement();
                            var nameText = template.GetChildrenEntityByName("name") as UILabel;
                            var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                            scrollView._rect.width = 300;
                            template._rect.width = 300;
                            nameText._rect.width = 290;
                            paramLabel._rect.width = 290;
                            panel._rect.width = 300;
                            panel._rect.height = 400;
                            name._rect.width = 300;
                            glass._rect.width = 300;
                            glass._rect.height = 400;

                            SchemeInventoryObjectInfo sInfo = info as SchemeInventoryObjectInfo;

                            var modelData = DataResources.Instance.ModuleData(sInfo.TargetTemplateId);

                            name._text = "Scheme";

                            nameText._text = "Name: ";
                            paramLabel._text = DataResources.Instance.NameModelSet(modelData.SetId);
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Level: ";
                            paramLabel._text = sInfo.Level.ToString();
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Workshop: ";
                            paramLabel._text = sInfo.Workshop.ToString();
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Type: ";
                            paramLabel._text = modelData.SlotType.ToString();
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);


                            foreach (KeyValuePair<string, int> sMaterial in sInfo.CraftMaterials)
                            {

                                paramLabel._text = sMaterial.Value.ToString();
                                ClientInventoryItem cItem;
                                int count = 0;
                                if (G.Game.Inventory.TryGetItem(InventoryObjectType.Material, sMaterial.Key, out cItem))
                                {
                                    count += cItem.Count;
                                }
                                if (G.Game.Station.StationInventory.TryGetItem(InventoryObjectType.Material, sMaterial.Key, out cItem))
                                {
                                    count += cItem.Count;
                                }

                                paramLabel.SetTextColor(sMaterial.Value > count ? Color.red : Color.green);
                                paramLabel._text = paramLabel._text + " / " + count;

                                nameText._text = DataResources.Instance.OreData(sMaterial.Key).Name + ": ";
                                scrollView.AddChild(template.Copy as UIScrollItemTemplate);
                            }

                        }
                        break;
                    case Common.InventoryObjectType.Material:
                        {


                            var template = scrollView.CreateElement();
                            var nameText = template.GetChildrenEntityByName("name") as UILabel;
                            var paramLabel = template.GetChildrenEntityByName("param") as UILabel;


                            scrollView._rect.width = 300;
                            template._rect.width = 300;
                            nameText._rect.width = 290;
                            paramLabel._rect.width = 290;
                            panel._rect.width = 300;
                            panel._rect.height = 400;
                            name._rect.width = 300;
                            glass._rect.width = 300;
                            glass._rect.height = 400;

                            MaterialInventoryObjectInfo mInfo = info as MaterialInventoryObjectInfo;

                            name._text = "Material";

                            nameText._text = "Name: ";
                            paramLabel._text = mInfo.Name;
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Level: ";
                            paramLabel._text = mInfo.Level.ToString();
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                            nameText._text = "Type: ";
                            paramLabel._text = mInfo.MaterialType.ToString();
                            scrollView.AddChild(template.Copy as UIScrollItemTemplate);
                        }
                        break;

                }

            }
            else if (typeof(T).Name == typeof(ClientShipModule).Name)
            {
                ClientShipModule info = (ClientShipModule)objInfo;
                ClientShipModule playerModule = G.Game.Ship.ShipModel.Module(info.type);

                var template = scrollView.CreateElement();
                var nameText = template.GetChildrenEntityByName("name") as UILabel;
                var paramLabel = template.GetChildrenEntityByName("param") as UILabel;

                

                scrollView._rect.width = 500;
                template._rect.width = 500;
                nameText._rect.width = 490;
                paramLabel._rect.width = 490;
                panel._rect.width = 500;
                panel._rect.height = 700;
                name._rect.width = 500;
                glass._rect.width = 500;
                glass._rect.height = 700;

                name._style = new GUIStyle(name._style);
                name._style.normal.textColor = Utils.GetColor(info.color);
                name._text = "Module";

                nameText._text = "Name: ";
                paramLabel.SetTextColor(Color.white);
                paramLabel._text = info.name;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "workshop: ";
                paramLabel.SetTextColor(Color.white);
                paramLabel._text = info.workshop.ToString();
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Level: ";
                paramLabel.SetTextColor(Color.white);
                paramLabel._text = info.level.ToString();
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                int delta;

                nameText._text = "Weapon slots count: ";
                delta = (int)(info.weaponSlotsCount - playerModule.weaponSlotsCount);
                paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                paramLabel._text = info.weaponSlotsCount + "    " + (delta < 0 ? "" : "+") + "" + delta;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Health points : ";

                delta = +(int)(info.hp - playerModule.hp);
                paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                paramLabel._text = ((int)info.hp) + "    " + (delta < 0 ? "" : "+") + "" + delta;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Hold: ";

                delta = +(int)(info.hold - playerModule.hold);
                paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                paramLabel._text = (int)info.hold + "    " + (delta < 0 ? "" : "+") + "" + delta;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Resists: ";

                delta = +(int)(info.resist - playerModule.resist);
                paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                paramLabel._text = (int)info.resist + "%" + "    " + (delta < 0 ? "" : "+") + "" + delta + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Speed: ";

                delta = +(int)(info.speed - playerModule.speed);
                paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                paramLabel._text = ((int)info.speed) + "    " + (delta < 0 ? "" : "+") + "" + delta;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Damage bonus: ";

                delta = (int)(info.damageBonus - playerModule.damageBonus);
                paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                paramLabel._text = (int)info.damageBonus + "%" + "    " + (delta < 0 ? "" : "+") + "" + delta + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Cooldown bonus: ";

                delta =  (int)(info.cooldownBonus - playerModule.cooldownBonus);
                paramLabel.SetTextColor(delta > 0 ? Color.red : Color.green);
                paramLabel._text = (int)info.cooldownBonus + "%" + "    " + (delta < 0 ? "" : "+") + "" + delta + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Distance bonus: ";

                delta = (int)(info.distanceBonus - playerModule.distanceBonus);
                paramLabel.SetTextColor(delta < 0 ? Color.red : Color.green);
                paramLabel._text = (int)info.distanceBonus + "%" + "    " + (delta < 0 ? "" : "+") + "" + delta + "%";
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

                nameText._text = "Weight: ";

                delta = (int)(playerModule.weight - info.weight);
                paramLabel.SetTextColor(delta > 0 ? Color.red : Color.green);
                paramLabel._text = ((int)info.weight) + "    " + (delta < 0 ? "" : "+") + "" + delta;
                scrollView.AddChild(template.Copy as UIScrollItemTemplate);

				
				nameText._text = "Set: ";
				paramLabel.SetTextColor(Color.white);
				paramLabel._text = info.set;
				scrollView.AddChild(template.Copy as UIScrollItemTemplate);

				if(info.HasSkill)
				{	
					var skillData = DataResources.Instance.SkillData(info.skill);
					if (skillData != null)
					{
						
						skillText.SetText("Skill: ");
						skillicon._texture =  TextureCache.Get("UI/Textures/Skills/" + info.skill);
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
            UIEntity panel = UIManager.Get.GetLayout("item_info");
            panel._visible = false;
        }

        public static void ToMousePosition()
        {
            UIEntity panel = UIManager.Get.GetLayout("item_info");
            Vector3 pos = Utils.ConvertedMousePosition;
            if (pos.x > Screen.width / 2 / Utils.GameMatrix().m00)
            {
                panel._rect.x = pos.x - panel._rect.width;
            }
            else
            {
                panel._rect.x = pos.x;
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

        public static void ShowWhenMouseEnterOnSource<T>(object itemObject, Texture2D icon)
        {
            SetupItemInfo.Setup<T>(itemObject, icon);
            SetupItemInfo.ToMousePosition();
            if( (itemObject as IInventoryObjectInfo).Type == InventoryObjectType.Weapon)
            {
                SetupItemInfoOnPlayer.Setup<IInventoryObjectInfo>(itemObject, icon);
                SetupItemInfoOnPlayer.ToMousePosition();
            }
            else
            {
                //hide  info player panel for non weapon items,  if that still opened by some reasons....
                if(SetupItemInfoOnPlayer.Visible)
                {
                    SetupItemInfoOnPlayer.Hide();
                }
            }
        }
        
        public static void HideWhenMouseExitFromSource()
        {
            SetupItemInfo.Hide();
            SetupItemInfoOnPlayer.Hide();
        }
    }
}
*/

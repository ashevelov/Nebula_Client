//using Common;
//using Nebula;
//using Nebula.Client.Inventory;
//using Nebula.Client.Inventory.Objects;
//using Nebula.Client.Res;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.Space.UI
//{
//    public class ModuleCraftingView 
//    {
//        private UIManager ui;
//        private UIContainerEntity root;
//        private UIScrollView scroll;
//        private UIButton closeButton;

//        private Dictionary<ShipModelSlotType, SchemeInventoryObjectInfo> selectedScehmes;
//        private Texture2D transparentTexture;

//        public ModuleCraftingView(UIManager ui)
//        {
//            this.selectedScehmes = new Dictionary<ShipModelSlotType, SchemeInventoryObjectInfo>();
//            this.selectedScehmes.Add(ShipModelSlotType.CB, null);
//            this.selectedScehmes.Add(ShipModelSlotType.CM, null);
//            this.selectedScehmes.Add(ShipModelSlotType.DF, null);
//            this.selectedScehmes.Add(ShipModelSlotType.DM, null);
//            this.selectedScehmes.Add(ShipModelSlotType.ES, null);

//            this.ui = ui;
//            this.root = this.ui.GetLayout("module_crafting") as UIContainerEntity;
//            this.scroll = this.root.GetChildrenEntityByName("crafting_scroll") as UIScrollView;
//            this.root.RegisterUpdate(Update);
//            this.transparentTexture = TextureCache.Get("UI/Textures/transparent");
//            this.closeButton = this.root.GetChildrenEntityByName("close") as UIButton;
//            this.closeButton.RegisterHandler(OnClose);
//        }

//        private ShipModelSlotType Index2SlotType(int index)
//        {
//            switch (index)
//            {
//                case 0:
//                    return ShipModelSlotType.CB;
//                case 1:
//                    return ShipModelSlotType.CM;
//                case 2:
//                    return ShipModelSlotType.DF;
//                case 3:
//                    return ShipModelSlotType.DM;
//                case 4:
//                    return ShipModelSlotType.ES;
//                default:
//                    return ShipModelSlotType.CB;
//            }
//        }

//        private Texture2D tempSchemeTexture;
//        private Texture2D GetSchemeIcon(ShipModelSlotType slotType)
//        {
//            if (!tempSchemeTexture)
//            {
//                tempSchemeTexture = TextureCache.Get("UI/Textures/schemes");
//            }
//            return tempSchemeTexture;
//        }

//        void ResetScrollItem(UIScrollItemTemplate t)
//        {
//            var nameLabel = t.GetChildrenEntityByName("name") as UILabel;
//            nameLabel.SetText(string.Empty);
//            var levelLabel = t.GetChildrenEntityByName("level_value") as UILabel;
//            levelLabel.SetText(string.Empty);
//            var weaponSlotsCountLabel = t.GetChildrenEntityByName("weapon_slots_count") as UILabel;
//            weaponSlotsCountLabel.SetText(string.Empty);
//            var resName = t.GetChildrenEntityByName("first_res_name") as UILabel;
//            var resCount = t.GetChildrenEntityByName("first_res_count") as UILabel;
//            resName.SetText(string.Empty);
//            resCount.SetText(string.Empty);
//            resName = t.GetChildrenEntityByName("second_res_name") as UILabel;
//            resCount = t.GetChildrenEntityByName("second_res_count") as UILabel;
//            resName.SetText(string.Empty);
//            resCount.SetText(string.Empty);
//            UIButton craftButton = t.GetChildrenEntityByName("craft_module") as UIButton;
//            craftButton.SetEnabled(false, true);
//        }

//        void Update(UIContainerEntity e)
//        {

//            this.scroll.Clear();

//            for (int i = 0; i < 5; i++)
//            {
//                UIScrollItemTemplate t = this.scroll.CreateElement();
//                UIButton selectSchemeButton = t.GetChildrenEntityByName("select_scheme") as UIButton;
//                selectSchemeButton.SetTag(this.Index2SlotType(i));
//                selectSchemeButton.RegisterHandler(OnSelectScheme);

//                var slotType = this.Index2SlotType(i);
//                var scheme = this.selectedScehmes[slotType];

//                if (scheme != null)
//                {
//                    var module = DataResources.Instance.ModuleData(scheme.TargetTemplateId);

//                    if (module != null)
//                    {
//                        UITexture tex = t.GetChildrenEntityByName("selected_scheme") as UITexture;
//                        tex.SetTexture(this.GetSchemeIcon(slotType));
//                        var nameLabel = t.GetChildrenEntityByName("name") as UILabel;
//                        nameLabel.SetText(module.Workshop.ToString() + "-" + module.SlotType.ToString());
//                        var levelLabel = t.GetChildrenEntityByName("level_value") as UILabel;
//                        levelLabel.SetText(": " + scheme.Level.ToString());
//                        var weaponSlotsCountLabel = t.GetChildrenEntityByName("weapon_slots_count") as UILabel;
//                        //weaponSlotsCountLabel.SetText(": " + module.WeaponSlots.ToString());

//                        ResMaterialData firstMaterial = null;
//                        ResMaterialData secondMaterial = null;
//                        foreach (var pair in scheme.CraftMaterials)
//                        {
//                            if (firstMaterial == null)
//                            {
//                                firstMaterial = DataResources.Instance.OreData(pair.Key);
//                            }
//                            else if (secondMaterial == null)
//                            {
//                                secondMaterial = DataResources.Instance.OreData(pair.Key);
//                            }
//                        }

//                        bool allowedFirst = false;
//                        bool allowedSecond = false;

//                        if (firstMaterial != null)
//                        {
//                            int needCount = scheme.CraftMaterials[firstMaterial.Id];
//                            int playerCount = 0;
//                            ClientInventoryItem item;
//                            if (G.Game.Inventory.TryGetItem(InventoryObjectType.Material, firstMaterial.Id, out item))
//                            {
//                                playerCount = item.Count;
//                            }

//                            if (playerCount >= needCount)
//                            {
//                                allowedFirst = true;
//                            }

//                            var resName = t.GetChildrenEntityByName("first_res_name") as UILabel;
//                            var resCount = t.GetChildrenEntityByName("first_res_count") as UILabel;
//                            if (allowedFirst)
//                            {
//                                resName.SetUseCustomColor(true, Color.green);
//                                resCount.SetUseCustomColor(true, Color.green);
//                            }
//                            else
//                            {
//                                resName.SetUseCustomColor(true, Color.red);
//                                resCount.SetUseCustomColor(true, Color.red);
//                            }
//                            resName.SetText(firstMaterial.Name);
//                            resCount.SetText(": " + needCount + "/" + playerCount);

//                        }

//                        if (secondMaterial != null)
//                        {
//                            int needCount = scheme.CraftMaterials[secondMaterial.Id];
//                            int playerCount = 0;
//                            ClientInventoryItem item;
//                            if (G.Game.Inventory.TryGetItem(InventoryObjectType.Material, secondMaterial.Id, out item))
//                            {
//                                playerCount = item.Count;
//                            }
//                            if (playerCount >= needCount)
//                            {
//                                allowedSecond = true;
//                            }

//                            var resName = t.GetChildrenEntityByName("second_res_name") as UILabel;
//                            var resCount = t.GetChildrenEntityByName("second_res_count") as UILabel;
//                            if (allowedSecond)
//                            {
//                                resName.SetUseCustomColor(true, Color.green);
//                                resCount.SetUseCustomColor(true, Color.green);
//                            }
//                            else
//                            {
//                                resName.SetUseCustomColor(true, Color.red);
//                                resCount.SetUseCustomColor(true, Color.red);
//                            }
//                            resName.SetText(secondMaterial.Name);
//                            resCount.SetText(": " + needCount + "/" + playerCount);
//                        }

//                        if (allowedFirst && allowedSecond)
//                        {
//                            UIButton craftButton = t.GetChildrenEntityByName("craft_module") as UIButton;
//                            craftButton.SetTag(scheme);
//                            craftButton.RegisterHandler(OnCraftScheme);
//                        }
//                        else
//                        {
//                            UIButton craftButton = t.GetChildrenEntityByName("craft_module") as UIButton;
//                            craftButton.SetEnabled(false, true);
//                        }
//                    }
//                    else
//                    {
//                        this.ResetScrollItem(t);
//                    }
//                }
//                else
//                {
//                    this.ResetScrollItem(t);
//                }
//                this.scroll.AddChild(t);
//            }
//        }


//        public void SetSelectedScheme(SchemeInventoryObjectInfo scheme)
//        {
//            if (scheme != null)
//            {
//                var moduleData = DataResources.Instance.ModuleData(scheme.TargetTemplateId);
//                if (moduleData != null)
//                {
//                    this.selectedScehmes[moduleData.SlotType] = scheme;
//                }
//            }
//        }


//        void OnSelectScheme(UIEvent evt)
//        {
//            //G.UI.SelectInventoryObjectView.ShowWithSchemes((ShipModelSlotType)evt._sender.tag, () => 
//            //{
//            //    if (G.UI.SelectInventoryObjectView.Selection != null)
//            //    {
//            //        SchemeInventoryObjectInfo scheme = G.UI.SelectInventoryObjectView.Selection as SchemeInventoryObjectInfo;
//            //        var module = DataResources.Instance.ModuleData(scheme.TargetTemplateId);
//            //        this.selectedScehmes[module.SlotType] = scheme;
//            //    }
//            //});
//        }

//        void OnCraftScheme(UIEvent evt)
//        {
//			ActionProgress.Setup(1, ()=>{
//				SchemeInventoryObjectInfo schemeInfo = evt._sender.tag as SchemeInventoryObjectInfo;
//				var module = DataResources.Instance.ModuleData(schemeInfo.TargetTemplateId);
//				NRPC.TransformInventoryObjectAndMoveToStationHold(schemeInfo.Type, schemeInfo.Id);
//				this.selectedScehmes[module.SlotType] = null;
//			});
//		}
		
//		void OnClose(UIEvent evt)
//        {
//            foreach (var ev in System.Enum.GetValues(typeof(ShipModelSlotType)))
//            {
//                this.selectedScehmes[(ShipModelSlotType)ev] = null;
//            }
//            this.root.SetVisibility(false);
//        }

//        public void Show(bool value)
//        {
//            this.root.SetVisibility(value);
//        }

//        public bool Visible
//        {
//            get { return this.root.Visible; }
//        }
//    }
//}


//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Common;
//using Game.Space;
//using Nebula.Client.Inventory;
//using Nebula.Client.Inventory.Objects;
//using Nebula;

//namespace Game.Space.UI
//{
//    public class WorkhousePlayerInventory
//    {
//        private UIManager uiManager;
//        private UIGroup root;
//        private UIScrollView scroll;
//        private UIButton actionButton;


//        private Dictionary<InventoryObjectType, Texture2D> textures;
//        private IInventoryObjectInfo selectedInfo;

//        public WorkhousePlayerInventory(UIManager uiManager)
//        {
//            this.textures = new Dictionary<InventoryObjectType, Texture2D>();
//            this.uiManager = uiManager;
//            this.root = uiManager.GetLayout("workhouse_player_inventory") as UIGroup;
//            this.scroll = this.root.GetChildrenEntityByName("scroll_items") as UIScrollView;
//            this.scroll.SetUpdateInterval(0.5f);

//            this.scroll.RegisterUpdate((p) => 
//            { 
//                FillScroll();
//                if (this.selectedInfo == null)
//                {
//                    this.actionButton.SetVisibility(false);
//                }
//                else
//                {
//                    this.actionButton.SetVisibility(true);
//                }
//            });

//            this.actionButton = this.root.GetChildrenEntityByName("action_button") as UIButton;

//            uiManager.RegisterEventHandler("WPI_TOGGLE_CHANGED", null, OnToggleClicked);
//            uiManager.RegisterEventHandler("WPI_ACTION_BUTTON", null, OnActionButtonClicked);
//        }

//        private void FillScroll()
//        {
//            this.scroll.Clear();
            
//            foreach (var typedItems in MmoEngine.Get.Game.Inventory.Items)
//            {
//                foreach (var itemPair in typedItems.Value)
//                {
//                    var inventoryItem = itemPair.Value;
//                    var template = this.scroll.CreateElement();
//                    var icon = template.GetChildrenEntityByName("icon") as UITexture;
//                    icon.SetTexture(TextureForItem(inventoryItem));
//                    var typeLabel = template.GetChildrenEntityByName("type") as UILabel;
//                    typeLabel.SetText(TypeTextForItem(inventoryItem));
//                    var nameLabel = template.GetChildrenEntityByName("name") as UILabel;
//                    nameLabel.SetText(NameTextForItem(inventoryItem));
//                    var countLabel = template.GetChildrenEntityByName("count") as UILabel;
//                    countLabel.SetText(inventoryItem.Count.ToString());
//                    countLabel.SetUseCustomColor(true, Color.green);

//                    var toggle = template.GetChildrenEntityByName("select_item") as UIToggle;
//                    toggle.SetTag(inventoryItem.Object);
//                    toggle.SetMouseRightButtonUpAction(OnToggleMouseRightUp);
//                    toggle.SetMouseLeftButtonUpAction(OnToggleMouseLeftUp);


//                    if (GetSelectedId() == inventoryItem.Object.Id)
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

//        private Texture2D TextureForItem(ClientInventoryItem item)
//        {
//            var type = item.Object.Type;
//            if (this.textures.ContainsKey(type))
//            {
//                return this.textures[type];
//            }
//            else
//            {
//                switch (type)
//                {
//                    case InventoryObjectType.Weapon:
//                        {
//                            this.textures[type] = TextureCache.Get("UI/Textures/weapon");
//                        }
//                        break;
//                    case InventoryObjectType.Scheme:
//                        {
//                            this.textures[type] = TextureCache.Get("UI/Textures/scheme");
//                        }
//                        break;
//                    case InventoryObjectType.Material:
//                        {
//                            this.textures[type] = TextureCache.Get("UI/Textures/ore");
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

//        private string TypeTextForItem(ClientInventoryItem item)
//        {
//            switch (item.Object.Type)
//            {
//                case InventoryObjectType.Weapon:
//                    return "weapon";
//                case InventoryObjectType.Scheme:
//                    return "scheme";
//                case InventoryObjectType.Material:
//                    {
//                        switch (((MaterialInventoryObjectInfo)item.Object).MaterialType)
//                        {
//                            case MaterialType.ore:
//                                return "Ore";
//                            default:
//                                return "Unknown material type";
//                        }
//                    }
//                default:
//                    return "TYPE UNKNOWN";
//            }
//        }

//        private string NameTextForItem(ClientInventoryItem item)
//        {
//            switch (item.Object.Type)
//            {
//                case InventoryObjectType.Weapon:
//                    {
//                        var weaponItem = item.Object as WeaponInventoryObjectInfo;
//                        var weaponTemplate = DataResources.Instance.Weapon(weaponItem.Template);
//                        return weaponTemplate.Name;
//                    }
//                case InventoryObjectType.Scheme:
//                    return ((SchemeInventoryObjectInfo)item.Object).Name;
//                case InventoryObjectType.Material:
//                    return ((MaterialInventoryObjectInfo)item.Object).Name;
//                default:
//                    return "UNKNOWN NAME";
//            }
//        }

//        private string GetSelectedId() 
//        {
//            if (selectedInfo == null)
//                return string.Empty;
//            else
//                return selectedInfo.Id;
//        }

//        private void SetSelection(IInventoryObjectInfo info)
//        {
//            this.selectedInfo = info;
//        }


//        private void OnToggleClicked(UIEvent evt)
//        {
//            Debug.Log("on click on toggle");
//            if (evt._parameters != null)
//            {
//                bool toggled = evt._parameters.GetValue<bool>("new_value", false);
//                var toggle = evt._sender as UIToggle;
//                if (toggled && toggle != null)
//                {
//                    SetSelection(toggle.tag as IInventoryObjectInfo);
//                    this.ToggleOffExcept((toggle.tag as IInventoryObjectInfo).Id);
//                    switch ((toggle.tag as IInventoryObjectInfo).Type)
//                    {
//                        case InventoryObjectType.Scheme:
//                            this.actionButton.SetText("TRANSFORM");
//                            break;
//                        case InventoryObjectType.Weapon:
//                            this.actionButton.SetText("UNKNOWN ACTION");
//                            break;
//                        default:
//                            this.actionButton.SetText("UNKNOWN ITEM");
//                            break;
//                    }
//                }
//                else
//                {
//                    this.ToggleOffAll();
//                }
//            }
//        }

//        private void OnActionButtonClicked(UIEvent evt)
//        {
//            if (this.selectedInfo != null)
//            {
//                switch (this.selectedInfo.Type)
//                {
//                    case InventoryObjectType.Scheme:
//                        {
//                            NRPC.TransformInventoryObjectAndMoveToStationHold(this.selectedInfo.Type, this.selectedInfo.Id);
//                            this.selectedInfo = null;
//                        }
//                        break;
//                    default:
//                        {
//                            "unsupported transformation".Print();
//                        }
//                        break;
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

//        private void ToggleOffExcept(string infoId)
//        {
//            foreach (var orderedChildrens in this.scroll._childrens)
//            {
//                foreach (var elem in orderedChildrens.Value)
//                {
//                    var toggle = elem.GetChildrenEntityByName("select_item") as UIToggle;
//                    if (toggle != null && toggle.tag != null)
//                    {
//                        if ((toggle.tag as IInventoryObjectInfo).Id == infoId)
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

//            this.actionButton.SetText(string.Empty);
//        }
//    }
//}

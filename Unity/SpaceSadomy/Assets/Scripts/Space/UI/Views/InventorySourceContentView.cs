// InventorySourceContentView.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Sunday, November 23, 2014 6:32:52 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//

    /*
namespace Game.Space.UI
{
    using UnityEngine;
    using System.Collections;
    using Common;
    using System.Collections.Generic;
    using Nebula.Client.Inventory;
    using Nebula.Client.Inventory.Objects;
    using Nebula;
    using Nebula.UI;

    public class InventorySourceContentView 
    {
        private UIManager manager;
        private UIContainerEntity root;
        private UIButton closeButton;
        private IInventoryItemsSource source;

        private UIContainerEntity contentParent;
        private GUIStyle cellStyle;
        private GUIStyle labelStyle;


        private readonly Size cellSize = new Size(60, 60);
        private readonly Size elementSize = new Size(50, 50);
        private readonly int cellsInRow = 4;
        private readonly TextureSubCache<string> subCache = new TextureSubCache<string>();


        public InventorySourceContentView(UIManager manager )
        {
            this.manager = manager;
            this.root = manager.GetLayout("inventory_source_content_view") as UIContainerEntity;
            this.closeButton = this.root.GetChild<UIButton>("close_button");
            this.contentParent = this.root.GetChild<UIContainerEntity>("content");
            this.cellStyle = this.manager.GetSkin("game").GetStyle("cell_button");
            this.labelStyle = this.manager.GetSkin("game").GetStyle("font_middle_left");
            this.root.GetChild<UIButton>("take_all").RegisterHandler(evt =>
                {
                    if (G.Game.Avatar == null)
                    {
                        Dbg.Print("Avatar is null", "UI");
                        return;
                    }
                    if(G.Game.Avatar.ShipDestroyed)
                    {
                        Dbg.Print("Avatar ship destroyed", "UI");
                        return;
                    }
                    if(this.source == null)
                    {
                        Dbg.Print("No valid source", "UI");
                        return;
                    }

                    switch(source.SourceType)
                    {
                        case ItemType.Asteroid:
                            G.Game.RequestMoveAsteroidToInventory(this.source.Id);
                            break;
                        case ItemType.Chest:
                            G.Game.AddAllFromContainer(this.source.Id, this.source.SourceType.toByte());
                            break;
                    }
                    
                    this.Close();
                });
            this.root.RegisterUpdate(Update);
            this.closeButton.RegisterHandler(e => Close());
        }

        public void Close()
        {
            this.root.SetVisibility(false);
        }

        public void Show(IInventoryItemsSource source, Vector2 pos)
        {

            this.source = source;
            float curX = this.root.Rect.x;
            float curY = this.root.Rect.y;
            float width = this.root.Rect.width;
            float height = this.root.Rect.height;

            if(pos.x + width > Utils.NativeWidth)
            {
                pos.x -= (pos.x + width - Utils.NativeWidth);
            }
            if(pos.y + height > (Utils.NativeHeight - 150) )
            {
                pos.y -= (pos.y + height - Utils.NativeHeight + 150);
            }
            this.root.SetPixelRect(new Rect(pos.x, pos.y, width, height));
            this.root.SetVisibility(true);
            Debug.Log("Item source count: {0}".f(source.Items.Count));
        }

        private void Update(UIContainerEntity e)
        {

            Vector2 oldMinSize = ((UIResizeableGroup)this.root)._minSize;

            float newMinHeight = Mathf.Min(((UIResizeableGroup)this.root)._maxSize.y, 236 + this.ViewHeight);

            ((UIResizeableGroup)this.root)._minSize = new Vector2(oldMinSize.x, newMinHeight);

            if(this.root.Rect.height < newMinHeight )
            {
                this.root.SetRect(new Rect(this.root.Rect.x, this.root.Rect.y, this.root.Rect.width, newMinHeight), UIUnits.px);
            }

            this.contentParent.Clear();
            this.contentParent.SetRect(this.ContentRect, UIUnits.px);

            int cellXIndex = 0;
            int cellYIndex = 0;

            for(int i = 0; i < this.source.Items.Count; i++ )
            {
                this.CreateCellView(cellXIndex, cellYIndex, this.source.Items[i]);
                cellXIndex++;
                if(cellXIndex == cellsInRow)
                {
                    cellXIndex = 0;
                    cellYIndex++;
                }
            }
        }

        //Create single cell
        private void CreateCellView(int xIndex, int yIndex, ClientInventoryItem item)
        {
            Rect cellGroupRect = new Rect(xIndex * this.cellSize.Width, yIndex * this.cellSize.Height, this.cellSize.Width, this.cellSize.Height);
            UIGroup group = new UIGroup
            {
                _childrens = new Dictionary<int, List<UIEntity>>(),
                _enabled = true,
                _name = string.Empty,
                _parent = null,
                _rect = cellGroupRect,
                _visible = true,
                align = UIAlign.None
            };
            Rect buttonRect = new Rect(0, 0, this.cellSize.Width, this.cellSize.Height);
            UIButton cellButton = new UIButton
            {
                _enabled = true,
                _name = item.Object.Id,
                _parent = group,
                _style = cellStyle,
                _text = string.Empty,
                _visible = true,
                align = UIAlign.Center,
                blockMouse = true,
                selfDrawable = false,
                tag = item
            };
            
            cellButton.events = new List<UIEvent>
            { 
                new UIEvent 
                {
                    _name = "INVENTORY_SOURCE_CONTENT_VIEW_CELL_RIGHT_CLICK",
                    _parameters = new Hashtable(),
                    _sender = cellButton,
                    EventType= UIEventType.RightClick
                },
                new UIEvent
                {
                    _name = "INVENTORY_SOURCE_CONTENT_VIEW_CELL_LEFT_CLICK",
                    _parameters = new Hashtable(),
                    _sender = cellButton,
                    EventType= UIEventType.LeftClick
                },
                new UIEvent
                {
                    _name = "INVENTORY_SOURCE_CONTENT_VIEW_CELL_MOUSE_ENTER",
                    _parameters = new Hashtable(),
                    _sender = cellButton,
                    EventType = UIEventType.OnEnter
                },
                new UIEvent
                {
                    _name = "INVENTORY_SOURCE_CONTENT_VIEW_CELL_MOUSE_EXIT",
                    _parameters = new Hashtable(),
                    _sender = cellButton,
                     EventType = UIEventType.OnExit
                }
            };
            group.AddChild(cellButton);
            cellButton.SetRect(buttonRect, UIUnits.px);
            cellButton.RegisterHandler(UIEventType.RightClick, OnRightClickCell);
            cellButton.RegisterHandler(UIEventType.OnEnter, OnMouseCellEnter);
            cellButton.RegisterHandler(UIEventType.OnExit, OnMouseCellExit);
            cellButton.RegisterHandler(UIEventType.LeftClick, OnLeftClickCell);

            Texture2D colorTexture = TextureCache.ColorTexture(item.Object);
            if(colorTexture)
            {
                UITexture colorTexObj = new UITexture
                {
                    _enabled = true,
                    _material = null,
                    _name = item.Object.Id + "_Color",
                    _parent = null,
                    _texture = colorTexture,
                    _visible = true,
                    align = UIAlign.Center,
                    blockMouse = false,
                    selfDrawable = false,
                    zorder = 1
                };
                group.AddChild(colorTexObj);
                colorTexObj.SetRect(buttonRect, UIUnits.px);
            }

            Rect textureRect = new Rect(0, 0, this.elementSize.Width, this.elementSize.Height);
            UITexture elementTexture = new UITexture
            {
                _enabled = true,
                _material = null,
                _name = item.Object.Id + "_Tex",
                _parent = null,
                _texture = this.GetItemTexture(item),
                _visible = true,
                align = UIAlign.Center,
                blockMouse = true,
                selfDrawable = false,
                tag = item,
                zorder = 2
            };
            group.AddChild(elementTexture);
            elementTexture.SetRect(textureRect, UIUnits.px);

            UILabel label = new UILabel
            {
                _enabled = true,
                _name = string.Empty,
                _parent = null,
                _style = this.labelStyle,
                _text = item.Count.ToString(),
                _visible = true,
                _rect = new Rect(0, 0, 0, 0),
                margin = new UIMargin { bottom = 10, right = 10, left = 0, top = 0 }
            };
            group.AddChild(label);
            label.SetRect(new Rect(0, 0, 0, 0), UIUnits.px);
            this.contentParent.AddChild(group);
            group.SetRect(cellGroupRect, UIUnits.px);
        }

        private void OnRightClickCell(UIEvent evt )
        {
            if (evt._sender.tag == null)
            {
                Dbg.Print("event tag is null", "UI");
                return;
            }
            ClientInventoryItem inventoryItem = evt._sender.tag as ClientInventoryItem;
            if(inventoryItem == null )
            {
                Dbg.Print("event tag is not inventory item", "UI");
                return;
            }

            if (source != null)
            {
                switch(source.SourceType)
                {
                    case ItemType.Asteroid:
                        G.Game.RequestMoveAsteroidItemToInventory(source.Id, inventoryItem.Object.Id, inventoryItem.Object.Type.toByte());
                        break;
                    case ItemType.Chest:
                        G.Game.Avatar.AddToInventory(source.Id, source.SourceType.toByte(), inventoryItem.Object.Id);
                        break;
                }
                
            }
        }

        private void OnMouseCellEnter(UIEvent evt )
        {
            ShowItemInfo(evt);
        }

        private void OnMouseCellExit(UIEvent evt )
        {
            //SetupItemInfo.HideWhenMouseExitFromSource();
        }

        private void OnLeftClickCell(UIEvent evt)
        {
            ShowItemInfo(evt);
        }

        private void ShowItemInfo(UIEvent evt)
        {
            ClientInventoryItem inventoryItem = evt._sender.tag as ClientInventoryItem;
            if (inventoryItem == null)
            {
                Debug.Log("not item...");
                return;
            }

            var itemObject = inventoryItem.Object;
            if (itemObject == null)
            {
                Debug.Log("object in inventory item null...");
                return;
            }

            if (!(itemObject is IInventoryObjectInfo))
            {
                Debug.Log("item is not inventory object info...");
                return;
            }

            //SetupItemInfo.ShowWhenMouseEnterOnSource<IInventoryObjectInfo>(itemObject, TextureCache.TextureForItem(itemObject as IInventoryObjectInfo));
        }

        private float ViewHeight
        {
            get
            {
                if (this.source == null || this.source.Items.Count == 0)
                    return 0f;
                int countOfRows = Mathf.CeilToInt((float)this.source.Items.Count / (float)this.cellsInRow);
                return countOfRows * this.cellSize.Height;
            }
        }

        private Rect ContentRect
        {
            get
            {
                return new Rect(this.contentParent.Rect.x, this.contentParent.Rect.y, this.contentParent.Rect.width, this.ViewHeight);
            }
        }

        private Texture2D GetItemTexture(ClientInventoryItem item)
        {
            if (!item.Has)
                return subCache.GetTexture("transparent", "UI/Textures/transparent");

            switch(item.Object.Type)
            {
                case InventoryObjectType.Material:
                    {
                        MaterialInventoryObjectInfo materialInfo = item.Object as MaterialInventoryObjectInfo;
                        switch(materialInfo.MaterialType)
                        {
                            case MaterialType.ore:
                                return subCache.GetTexture(materialInfo.TemplateId, "UI/Textures/Ore/" + materialInfo.TemplateId);
                            default:
                                throw new System.Exception("not founded material item texture for material type: {0}".f(materialInfo.MaterialType));
                        }
                    }
                case InventoryObjectType.Scheme:
                    return subCache.GetTexture("schemes", "UI/Textures/schemes");
                case InventoryObjectType.Weapon:
                    {
                        WeaponInventoryObjectInfo weaponInfo = item.Object as WeaponInventoryObjectInfo;
                        if (string.IsNullOrEmpty(weaponInfo.Template))
                            throw new System.Exception("Not found template for weapon");
                        return subCache.GetTexture(weaponInfo.Template, "UI/Textures/" + weaponInfo.Template);
                    }
                default:
                    throw new System.Exception("inventory type: {0} not supported".f(item.Object.Type));
            }
        }
    }

}
*/
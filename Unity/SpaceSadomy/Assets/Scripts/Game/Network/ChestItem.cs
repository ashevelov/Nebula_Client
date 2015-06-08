using Common;
using Nebula.Client.Inventory;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nebula.UI;
using System;
using Nebula.Mmo.Games;

namespace Nebula
{

    public class ChestItem : Item, IInventoryItemsSource, Nebula.UI.ISelectedObjectContextMenuViewSource {
        private Chest _component;
        private List<ClientInventoryItem> items = new List<ClientInventoryItem>();

        public ChestItem(string id, byte type, NetworkGame game, string name, object[] inComponents)
            : base(id, type, game, name, inComponents)
        { 
            
        }

        /*
        public override void CreateView(GameObject prefab)
        {
            base.CreateView(prefab);
            _component = View.AddComponent<Chest>();
            _component.Initialize(Game, this);
        }*/

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            _component = View.AddComponent<Chest>();
            _component.Initialize(Game, this);
        }

        public override bool IsMine
        {
            get 
            {
                return false;
            }
        }

        //no implementation for chest
        public override void UseSkill(Hashtable skillProperties)
        {
            //not imple
        }

        public override BaseSpaceObject Component
        {
            get 
            {
                return _component;
            }
        }

        public override void AdditionalUpdate()
        {
            
        }

        public System.Collections.Generic.List<ClientInventoryItem> Items
        {
            get { return this.items; }
        }


        public void SetItems(System.Collections.Generic.List<ClientInventoryItem> items)
        {
            this.items = items;
            if(items.Count == 0 )
            {
                if(this._component)
                {
                    if(this._component.ChildrensActive)
                    {
                        this._component.DeactivateChildrens();
                    }
                }
            }
            else
            {
                if(this._component)
                {
                    if(!this._component.ChildrensActive)
                    {
                        this._component.ActivateChildrens();
                    }
                }
            }
        }

        public SelectedObjectContextMenuView.InputData ContextViewData() {
            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
//                new SelectedObjectContextMenuView.InputEntry {
//                     ButtonText = "Info",
//                      ButtonAction = ()=> {
//                          Debug.Log("Show chest info");
//                          MainCanvas.Get.Show(CanvasPanelType.InventorySourceView, this);
//                      }
//                },
                new SelectedObjectContextMenuView.InputEntry {
                    ButtonText = "Collect",
                    ButtonAction = ()=> 
					{
						Debug.Log("Collect chest content");
						MainCanvas.Get.Show(CanvasPanelType.InventorySourceView, this);
					}
                }
            };
            return new SelectedObjectContextMenuView.InputData {
                TargetItem = this,
                Inputs = entries
            };
        }

        public bool RemoveItem(string itemId) {
            var index = this.items.FindIndex(it => it.Object.Id == itemId);
            if(index >= 0 ) {
                this.items.RemoveAt(index);
                return true;
            }
            return false;
        }

        public Common.ItemType SourceType
        {
            get { return this.Type.toItemType(); }
        }
    }
}

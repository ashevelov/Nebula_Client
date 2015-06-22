namespace Nebula.Mmo.Items
{
    using Common;
    using Nebula.Client.Inventory;
    using Nebula.Client.Inventory.Objects;
    using Nebula.UI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using ServerClientCommon;
    using Nebula.Mmo.Games;
    using Nebula.Resources;

    public class AsteroidItem : ForeignItem, IInventoryItemsSource, IAsteroidObjectInfo, Nebula.UI.ISelectedObjectContextMenuViewSource {

        //private TextureSubCache<string> textureSubCache = new TextureSubCache<string>();

        public class AsteroidContent
        {
            public int Count { get; set; }
            public MaterialInventoryObjectInfo Material { get; set; }
        }

        private List<AsteroidContent> content = new List<AsteroidContent>();


        private BaseSpaceObject component;

        public AsteroidItem(string id, byte type, NetworkGame game, string name, object[] inComponents)
            : base(id, type, game, name, inComponents)
        {

        }

        /*
        public override void CreateView(GameObject prefab)
        {
            base.CreateView(prefab);
            this.component = this._view.AddComponent<Asteroid>();
            this.component.Initialize(this.Game, this);
        }*/

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            this.component = this._view.AddComponent<Asteroid>();
            this.component.Initialize(this.Game, this);
        }

        public override BaseSpaceObject Component
        {
            get { return this.component; }
        }

        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.AsteroidContent:
                    {
                        this.SetAsteroidContent(newValue);
                    }
                    break;
                case PS.AsteroidData:
                    {
                        this.ReplaceName(newValue.ToString());
                    }
                    break;
            }
        }


        public void SetAsteroidContent(object newValue)
        {
            object[] contentArray = newValue as object[];
            this.content.Clear();
            foreach (object obj in contentArray)
            {
                Hashtable objInfo = obj as Hashtable;
                int count = objInfo.GetValue<int>((int)SPC.Count, 0);
                MaterialInventoryObjectInfo material = new MaterialInventoryObjectInfo(objInfo.GetValue<Hashtable>((int)SPC.Info, new Hashtable()));
                AsteroidContent c = new AsteroidContent { Count = count, Material = material };
                this.content.Add(c);
            }
            Events.EvtInventoryItemSourceUpdated(this);
        }

       

        public List<AsteroidContent> Content
        {
            get
            {
                return this.content;
            }
        }


        public override void DestroyView()
        {
            if (this.ExistsView)
            {
                this._view.GetComponent<AlphaFadeDestroy>().DestroyObject(1.0f);
                this._view = null;
            }
        }

        public override void AdditionalUpdate()
        {

        }

        public List<ClientInventoryItem> Items
        {
            get
            {
                List<ClientInventoryItem> items = new List<ClientInventoryItem>();
                foreach (var c in content)
                {
                    items.Add(new ClientInventoryItem(c.Material, c.Count));
                }
                return items;
            }
        }

        public List<Sprite> ContentSprites
        {
            get
            {
                List<Sprite> contentTextures = new List<Sprite>();
                foreach (var c in this.content)
                {
                    contentTextures.Add(SpriteCache.OreSprite(c.Material.Id));
                }
                return contentTextures;
            }
        }

        public Sprite Icon
        {
            get { return SpriteCache.TargetSprite("asteroid"); }
        }

        public ObjectInfoType InfoType
        {
            get { return ObjectInfoType.Asteroid; }
        }

        public string Description
        {
            get { return "Asteroid - a type of space resources, which is used for processing and production of valuable ores."; }
        }

        public Color Relation
        {
            get { return Color.white; }
        }


        public float DistanceToPlayer
        {
            get
            {
                if (G.Game != null)
                {
                    if (G.Game.Avatar != null)
                    {
                        if (G.Game.Avatar.Component)
                        {
                            if (this.component)
                            {
                                var playerPos = G.Game.Avatar.Component.transform.position;
                                var selfPos = this.component.transform.position;
                                return Vector3.Distance(playerPos, selfPos);
                            }

                        }
                    }
                }

                return float.NaN;
            }
        }


        public void SetItems(List<ClientInventoryItem> items)
        {

        }

        public SelectedObjectContextMenuView.InputData ContextViewData() {
            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
                //new SelectedObjectContextMenuView.InputEntry {
                //     ButtonText = "Info",
                //      ButtonAction = ()=> {
                //          Debug.LogFormat("Show asteroid content with number items: {0}", this.Items.Count);
                //          MainCanvas.Get.Show(CanvasPanelType.InventorySourceView, this);
                //      }
                //},
                new SelectedObjectContextMenuView.InputEntry {
                    ButtonText = "Collect",
                    ButtonAction = ()=> {
                          Debug.LogFormat("Show asteroid content with number items: {0}", this.Items.Count);
                          MainCanvas.Get.Show(CanvasPanelType.InventorySourceView, this);
                      }
                    //ButtonAction = ()=> {Debug.Log("Collect asteroid content"); }
                }
            };
            return new SelectedObjectContextMenuView.InputData {
                TargetItem = this,
                Inputs = entries
            };
        }

        public bool RemoveItem(string itemId) {
            int index = this.content.FindIndex((c) => c.Material.Id == itemId);
            if(index >= 0 ) {
                this.content.RemoveAt(index);
                return true;
            }
            return false;
        }

        public ItemType SourceType
        {
            get { return this.Type.toItemType(); }
        }
    }
}

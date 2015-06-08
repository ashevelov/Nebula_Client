using UnityEngine;
using System.Collections;
using Common;
using Game.Network;
using Nebula;
using ServerClientCommon;
using Nebula.Mmo.Games;

namespace Game.Space
{
    public class WorldActivatorItem : NpcItem
    {
        private BaseSpaceObject component;
        private bool active;
        private float radius;
        private int activatorType;
        private float nextUpdateTime;

        public WorldActivatorItem(string id, byte type, NetworkGame game, string name, object[] inComponents)
            : base(id, type, game, BotItemSubType.Activator, name, inComponents)
        {
            this.active = false;
            this.activatorType = -1;
            nextUpdateTime = Time.time;
        }

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            this.component = this._view.AddComponent<WorldActivator>();
            this.component.Initialize(this.Game, this);
        }

        public override bool IsMine
        {
            get { return false; }
        }

        public override void UseSkill(Hashtable skillProperties) { }

        public override BaseSpaceObject Component
        {
            get { return this.component; }
        }

        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.Info:
                    {
                        Hashtable infoProperties = newValue as Hashtable;
                        foreach (DictionaryEntry entry in infoProperties) {
                            switch ((SPC)(int)entry.Key) {
                                case SPC.Radius:
                                    {
                                        radius = (float)entry.Value;
                                        this.OnActivatorRadiusPropertyReceived();
                                    }
                                    break;
                                case SPC.Active:
                                    {
                                        active = (bool)entry.Value;
                                    }
                                    break;
                                case SPC.Type:
                                    {
                                        activatorType = (int)entry.Value;
                                    }
                                    break;
                            }
                        }
                        break;

                    }
            }
        }



        private void OnActivatorRadiusPropertyReceived()
        {
            if(this.HasView())
            {
                var sphereCollider = this.View.GetComponent<SphereCollider>();
                if(!sphereCollider)
                {
                    //Debug.LogError("This activator don't have sphere collider");
                    return;
                }
                sphereCollider.radius = this.Radius * .5f;
                //Debug.Log("set activator sphere radius to: {0}".f(sphereCollider.radius));
            }
        }

        public float Radius
        {
            get
            {
                return this.radius;
            }
        }

        public bool Active
        {
            get
            {
                return this.active;
            }
        }

        public int ActivatorType
        {
            get
            {
                return this.activatorType;
            }
        }

        public void TryActivate()
        {
            Debug.Log("Try activate");
            NRPC.Activate(this.Id);
        }

        public override void AdditionalUpdate()
        {
            if (Time.time > this.nextUpdateTime)
            {
                this.nextUpdateTime = Time.time + 5.0f;
                this.GetProperties();
            }
        }
    }
}


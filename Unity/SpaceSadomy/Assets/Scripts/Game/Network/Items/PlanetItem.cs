using UnityEngine;
using System.Collections;
using Game.Network;
using Common;
using Game.Space;
using Nebula.Client;

namespace Nebula.Game.Network.Items
{
    public class PlanetItem : NpcItem
    {
        private readonly ClientPlanetInfo planetInfo;

        public PlanetItem(string id, byte type, NetworkGame game, string name)
            : base(id, type, game, BotItemSubType.Planet, name)
        {
            this.planetInfo = new ClientPlanetInfo();
        }

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            this.SetComponent(this.View.AddComponent<Planet>());
            this.Component.Initialize(this.Game, this);
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch(group)
            {
                case GroupProps.planet:
                    {
                        switch(propName)
                        {
                            case Props.planet_info:
                                {
                                    Hashtable info = newValue as Hashtable;
                                    if(info != null )
                                    {
                                        this.planetInfo.ParseInfo(info);
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        public override void OnSettedGroupProperties(string group, Hashtable properties)
        {
            base.OnSettedGroupProperties(group, properties);
            foreach(DictionaryEntry propertyEntry in properties)
            {
                string propertyName = propertyEntry.Key.ToString();
                var oldPropertyValue = this.GetProperty(group, propertyName);
                this.OnSettedProperty(group, propertyName, propertyEntry.Value, oldPropertyValue);
            }
        }

        public ClientPlanetInfo PlanetInfo()
        {
            return this.planetInfo;
        }

        public override void DestroyView()
        {
            base.DestroyView();
        }

        public override void UseSkill(Hashtable skillProperties) { }
        public override void AdditionalUpdate() { }
    }
}

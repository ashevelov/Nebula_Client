﻿using Common;
using Nebula.Client;
using Nebula.Mmo.Games;
using System.Collections;
using UnityEngine;

namespace Nebula.Game.Network.Items {
    public class PlanetItem : NpcItem {
        private readonly ClientPlanetInfo planetInfo;

        public PlanetItem(string id, byte type, NetworkGame game, string name)
            : base(id, type, game, BotItemSubType.Planet, name) {
            this.planetInfo = new ClientPlanetInfo();
        }

        public override void Create(GameObject obj) {
            base.Create(obj);
            this.SetComponent(this.View.AddComponent<Planet>());
            this.Component.Initialize(this.Game, this);
        }

        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.PlanetInfo:
                    {
                        Hashtable info = newValue as Hashtable;
                        if(info != null ) {
                            planetInfo.ParseInfo(info);
                        }
                    }
                    break;
            }
        }


        public ClientPlanetInfo PlanetInfo() {
            return this.planetInfo;
        }

        public override void DestroyView() {
            base.DestroyView();
        }

        public override void UseSkill(Hashtable skillProperties) { }
        public override void AdditionalUpdate() { }
    }
}

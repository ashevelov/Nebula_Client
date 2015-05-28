using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Space;
using Common;

namespace Nebula {
    /// <summary>
    /// Modules on foreigh ship
    /// </summary>
    public class ForeignShipModules : IServerPropertyParser {

        private Dictionary<ShipModelSlotType, string> prefabs;

        public ForeignShipModules() {
            prefabs = new Dictionary<ShipModelSlotType, string>();
        }

        public void ParseProp(string propName, object value) {
            switch (propName) {
                case Props.SHIP_MODULES_PREFABS:
                    //Debug.Log("received modules for foreign player");
                    Hashtable info = value as Hashtable;
                    if (info != null) {
                        ParsePrefabs(info);
                    }
                    break;
            }
        }

        public void ParseProps(Hashtable properties) {
            foreach (DictionaryEntry entry in properties) {
                ParseProp(entry.Key.ToString(), entry.Value);
            }
        }

        private void ParsePrefabs(Hashtable prefabsInfo) {
            this.prefabs.Clear();
            foreach (DictionaryEntry e in prefabsInfo) {
                this.prefabs.Add(((byte)e.Key).toEnum<ShipModelSlotType>(), e.Value.ToString());
            }
        }

        public Dictionary<ShipModelSlotType, string> Prefabs {
            get { return prefabs; }
        }
    }
}
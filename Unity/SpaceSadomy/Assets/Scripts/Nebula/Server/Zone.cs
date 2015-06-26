namespace Nebula.Server {
    using UnityEngine;
    using System.Collections;
    using Common;

    [AddComponentMenu("Server/Objects/Zone")]
    public class Zone : MonoBehaviour {
        /// <summary>
        /// Unique ID of zone
        /// </summary>
        public string zoneID;
        
        /// <summary>
        /// ID of zone name
        /// </summary>
        public string zoneName;
        
        /// <summary>
        /// Zone level
        /// </summary>
        public int level;
        
        /// <summary>
        /// Race which owned zone at start
        /// </summary>
        public Race race;
    }
}

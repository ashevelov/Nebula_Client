namespace Nebula.Server {
    using UnityEngine;
    using System.Collections;

    [AddComponentMenu("Server/Objects/Asteroid")]
    public class ASTEROID : MonoBehaviour {

        public int asteroidID;
        public bool forceCreate;
        public string dataID;
        public float respawnInterval;
        public string modelID;


    }
}

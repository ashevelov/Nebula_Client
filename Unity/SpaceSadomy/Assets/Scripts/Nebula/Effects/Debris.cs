namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class Debris : MonoBehaviour {

        public GameObject debrisPrefab;
        public float debrisSpeed = -1;


        private List<GameObject> debrisInstances = new List<GameObject>();

        void Start() {
            for(int i = 0; i < 5; i++) {
                debrisInstances.Add(Instantiate(debrisPrefab, new Vector3(0, 0, 0 + i * 20), Quaternion.identity) as GameObject);
                debrisInstances[i].transform.SetParent(transform, true );
            }
        }

        void Update() {
            foreach(var obj in debrisInstances) {
                obj.transform.position += Vector3.forward * debrisSpeed * Time.deltaTime;
            }


            if(debrisInstances[0].transform.position.z < -30) {
                GameObject rObj = debrisInstances[0];
                debrisInstances.RemoveAt(0);
                Destroy(rObj);
                rObj = null;
            }

            var lastObj = debrisInstances[debrisInstances.Count - 1];
            if(lastObj.transform.position.z < 80) {
                debrisInstances.Add(Instantiate(debrisPrefab, new Vector3(0, 0, lastObj.transform.position.z + 20), Quaternion.identity) as GameObject);
                debrisInstances[debrisInstances.Count - 1].transform.SetParent(transform, true);
            }
        }
    }
}
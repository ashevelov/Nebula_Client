using UnityEngine;


public class LoreControler : MonoBehaviour {

	void Start () {
        if (Random.value < 0.1f)
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/LoreObject") as GameObject) as GameObject;
            go.transform.position = Random.insideUnitSphere * 1000;
        }
	}
}

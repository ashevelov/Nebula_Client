using Nebula;
using Nebula.Resources;
using System.Collections;
using UnityEngine;

public class ShipExp : MonoBehaviour {

	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Exp());
        }
	}
    private IEnumerator Exp()
    {
        GameObject obj1 = Instantiate(PrefabCache.Get("Prefabs/Effects/AirExplosion04"), transform.position + transform.forward*2, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(0.5f);
        GameObject obj2 = Instantiate(PrefabCache.Get("Prefabs/Effects/AirExplosion04"), transform.position - transform.forward * 2, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(0.5f);
        GameObject obj3 = Instantiate(PrefabCache.Get("Prefabs/Effects/AirExplosion03"), transform.position + transform.up, Quaternion.identity) as GameObject;
        Destroy(gameObject);
    }
}

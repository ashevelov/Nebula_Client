using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallExplosion : MonoBehaviour {

    public int fragmentsCount = 10;

    public GameObject fragment;
    public GameObject smallfragment;
    public List<Rigidbody> fragments = new List<Rigidbody>();

	void Start () {
        for (int i = 0; i < fragmentsCount; i++)
        {
            Rigidbody frag = (Instantiate(fragment) as GameObject).GetComponent<Rigidbody>();
            //frag.transform.parent = transform;
            frag.velocity = new Vector3(Random.Range(-250, 250), Random.Range(-250, 250), Random.Range(-250, 250));
            fragments.Add(frag);
            StartCoroutine(RemoveFragment(4, frag, true));
        }
        Destroy(gameObject, 10);
	}
    float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if (time < 1)
        {
            foreach (var f in fragments)
            {
                if (Random.value < 0.05f)
                {
                    Rigidbody frag = (Instantiate(smallfragment, f.transform.position, Quaternion.identity) as GameObject).GetComponent<Rigidbody>();
                    //frag.transform.parent = transform;
                    frag.velocity = f.velocity / 1f;
                    frag.velocity += new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
                    StartCoroutine(RemoveFragment(3, frag, false));
                }
                if (Random.value < 0.01f)
                {
                    f.velocity += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
                }
               // f.velocity = f.velocity *(Time.deltaTime * 10f);
            }
        }
    }

    private IEnumerator RemoveFragment(float time, Rigidbody go, bool big)
    {
        yield return new WaitForSeconds(time);
        if(big) fragments.Remove(go);
        Destroy(go.gameObject);
    }
	
}

using UnityEngine;
using System.Collections;

public class HelpArrow : MonoBehaviour {

    private Transform _target;

    public static void Init(Transform target)
    {
        HelpArrow a = FindObjectOfType<HelpArrow>();
        if (a != null)
        {
            a.SetTarget(target);
        }
        else
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/HelpArrow") as GameObject) as GameObject;
            MyPlayer mp = FindObjectOfType<MyPlayer>();
            if(mp != null)
            {
                go.transform.parent = mp.gameObject.transform;
            
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one / 2 *1000;
                a = go.GetComponent<HelpArrow>();
                a.SetTarget(target);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

	void Update ()
    {
        if (_target != null)
        {
            transform.LookAt(_target.position);
            if (Vector3.Distance(transform.position, _target.position) < 3000)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
	}
}

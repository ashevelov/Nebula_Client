using UnityEngine;
using System.Collections;

public class ShieldExp : MonoBehaviour {

    public Color _startColor;
    public float speed = 0.2f;
    public Transform effect;

    public static void Init(Transform target, Transform tParent)
    {
        GameObject go = Resources.Load("Prefabs/Effects/Shields/shieldExp") as GameObject;
        if (go != null)
        {
            GameObject inst = Instantiate(go) as GameObject;
            inst.transform.parent = tParent;
            inst.transform.localPosition = Vector3.zero;
            inst.transform.LookAt(target);
            Destroy(inst, 4);
        }
        else 
        {
            Debug.Log("game object not found");
        }
    }

    void Update()
    {
        _startColor.a -= speed * Time.deltaTime * (1.1f - _startColor.a);
        effect.GetComponent<Renderer>().material.color = _startColor;
        transform.position += transform.forward * Time.deltaTime*1f;
    }
}

using UnityEngine;
using System.Collections;

public class JumpGate : MonoBehaviour {
    public Transform target;

    public static void Init(Vector3 startPos, Vector3 EndPos)
    {
        GameObject go = Resources.Load("Prefabs/Effects/SkillsEffects/JumpGate") as GameObject;
        if (go != null)
        {
            GameObject inst1 = Instantiate(go) as GameObject;
            inst1.transform.position = startPos;
            GameObject inst2 = Instantiate(go) as GameObject;
            inst2.transform.position = EndPos;
            inst1.GetComponent<JumpGate>().target = inst2.transform;
            inst2.GetComponent<JumpGate>().target = inst1.transform;
        }
        else
        {
            Debug.Log("game object not found");
        }
    }

    void Start()
    {
        transform.LookAt(target);
        Destroy(gameObject, 2);
    }
    void Update()
    {
    }
}

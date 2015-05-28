using UnityEngine;
using System.Collections;

public class TestMissile : MonoBehaviour {
    public Transform target;
    public Missile missile;

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 30), "Start missile"))
        {
            missile.SetTarget(target, false);
        }
    }
}

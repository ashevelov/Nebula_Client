using UnityEngine;
using System.Collections;
using Game.Space;

public class AccelerationGate : Singleton<AccelerationGate> {

    [SerializeField]
    private Transform _outTransform;

    [SerializeField]
    private float outRadius = 5000.0f;

    public Vector3 GetOutPoint()
    {
        Vector3 rndVec = Random.insideUnitSphere * outRadius;
        return _outTransform.position + rndVec;
    }
}

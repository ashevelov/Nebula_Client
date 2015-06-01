using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using Nebula;

public class EdgeCube : MonoBehaviour {

    public float rotationSpeed = 1.0f;

    private Vector3 rotationAxis;
    private float angle;

	// Use this for initialization
	void Start () {
        this.rotationAxis = Random.insideUnitSphere;
        this.angle = 0;
        transform.rotation = Quaternion.AngleAxis(angle, rotationAxis);
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion rot = Quaternion.AngleAxis( this.angle, this.rotationAxis);
        this.angle += Time.deltaTime * this.rotationSpeed;
        transform.rotation = rot;
	}

    void OnTriggerEnter(Collider other)
    {
        string.Format("OnTriggerEnter(): {0}", other.name).Bold().Color(Color.green).Print();
        BaseSpaceObject spaceObj = other.GetComponent<BaseSpaceObject>();
        if (spaceObj)
        {
            if (spaceObj.Item.IsMine)
            {
                G.Game.EnterWorkshop( WorkshopStrategyType.Angar);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        string.Format("OnTriggerExit(): {0}", other.name).Bold().Color(Color.green).Print();
        var baseSpaceObj = other.GetComponent<BaseSpaceObject>();
        if (baseSpaceObj)
        {
            if (baseSpaceObj.Item.IsMine)
            {
                Operations.ExitWorkshop(G.Game);
            }
        }
    }
}

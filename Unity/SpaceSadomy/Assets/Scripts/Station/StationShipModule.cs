using UnityEngine;
using System.Collections;
using Common;

public class StationShipModule : MonoBehaviour {

    public ShipModelSlotType slotType;
    private Material mat;
    private MeshCollider collader;

    void Start()
    {
        collader = GetComponent<MeshCollider>();
        if (GetComponent<Collider>() == null)
        {
            collader = gameObject.AddComponent<MeshCollider>();            
        }
    }

    void OnMouseEnter() {
        if (mat == null)
            mat = this.GetComponent<Renderer>().material;
        if (mat == null)
            mat = this.GetComponentInChildren<MeshRenderer>().material;
        if (mat != null)
        {
            mat.SetFloat("_EmissionLM", 1.0f);
        }
    }

    void OnMouseExit()
    {
        if (mat == null)
            mat = this.GetComponent<Renderer>().material;
        if (mat == null)
            mat = this.GetComponentInChildren<MeshRenderer>().material;
        if (mat != null)
        {
            mat.SetFloat("_EmissionLM", 0.0f);
        }
    }
}

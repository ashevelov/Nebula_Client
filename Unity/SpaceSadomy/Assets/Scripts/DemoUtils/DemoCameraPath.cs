using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class DemoCameraPath : MonoBehaviour {

    private Transform[] childrens;

	// Use this for initialization
	void Start () 
    {
        //find all childrens path point
		if(false == Application.isPlaying) {
			this.Initialize();
		}

	}

    public void Initialize()
    {
        this.childrens = new Transform[transform.childCount];
        int index = 0;
        foreach (Transform t in transform)
        {
            this.childrens[index++] = t;
        }
    }
	
    public Transform GetPoint(int index)
    {
        return this.childrens[index % this.childrens.Length];
    }


    void OnDrawGizmos()
    {
        if(this.childrens != null )
        {
            for (int i = 0; i < this.childrens.Length; i++)
            {
                int curIndex = i;
                int nextIndex = (i == this.childrens.Length - 1) ? 0 : (i + 1);
                Gizmos.DrawLine(this.childrens[curIndex].position, this.childrens[nextIndex].position);
            }
        }
    }
}

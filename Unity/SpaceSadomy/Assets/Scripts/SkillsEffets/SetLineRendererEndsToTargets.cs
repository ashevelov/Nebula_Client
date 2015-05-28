using UnityEngine;
using System.Collections;

public class SetLineRendererEndsToTargets : MonoBehaviour {

    private LineRenderer lineRenderer;
    private Vector3 oldStart;
    private Vector3 oldEnd;

    private Transform start;
    private Transform end;


	void Start () 
    {
        this.lineRenderer = GetComponent<LineRenderer>();
        this.lineRenderer.SetVertexCount(2);
        oldStart = Vector3.zero;
        oldEnd = Vector3.zero;
        this.lineRenderer.SetPosition(0, oldStart);
        this.lineRenderer.SetPosition(1, oldEnd);
	}
	

	void Update () 
    {
        if (this.start)
        {
            this.oldStart = this.start.position;
            this.lineRenderer.SetPosition(0, this.oldStart);
        }
        else
        {
            this.lineRenderer.SetPosition(0, this.oldStart);
        }

        if (this.end)
        {
            this.oldEnd = this.end.position;
            this.lineRenderer.SetPosition(1, this.oldEnd);
        }
        else
        {
            this.lineRenderer.SetPosition(1, this.oldEnd);
        }
	}

    public void SetStartAndEnd(Transform startTransform, Transform endTransform)
    {
        this.start = startTransform;
        this.end = endTransform;
    }
}

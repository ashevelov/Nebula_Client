using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    private Transform target;
    private Transform source;
    private LineRenderer lineRenderer;
    private float timer;
    private bool showing;
    private bool visible;
    private bool hiding;
    private Vector3 _sourceOffset;

    public void Setup(Transform source, Transform target, Vector3 sourceOffset)
    {
        this.source = source;
        this.target = target;
        this.timer = 0.0f;
        visible = false;
        hiding = false;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, source.position);
        lineRenderer.SetPosition(1, target.position);
        lineRenderer.SetColors(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0));
        _sourceOffset = sourceOffset;
        showing = true;
    }

    void Update() {
        if (source && target)
        {
            lineRenderer.SetPosition(0, source.position + _sourceOffset);
            lineRenderer.SetPosition(1, target.position);
        }

        if (showing)
        {
            timer += Time.deltaTime;
            float ntimer = timer / 0.1f;
            Color c = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), ntimer);
            lineRenderer.SetColors(c, c);
            if (ntimer >= 1.0f)
            {
                showing = false;
                visible = true;
                timer = 0.0f;
            }
        }
        else if (visible)
        {
            timer += Time.deltaTime;
            float ntimer = timer / 0.3f;
            if (ntimer >= 1.0f)
            {
                visible = false;
                hiding = true;
                timer = 0.0f;
            }
        }
        else if (hiding)
        {
            timer += Time.deltaTime;
            float ntimer = timer / 0.1f;
            if (ntimer >= 1.0f)
            {
                hiding = false;
                timer = 0.0f;
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class ScaleOverTime : MonoBehaviour {

    public float stayTime = 5;
    public float scaleTime;
    public Vector3 targetScale;
    public bool destroyAfterStay = false;

    private Vector3 startScale;
    private float timer;
    private bool started = false;
    private bool stayStarted = false;
    private float stayTimer;

	void Start () 
    {
        this.startScale = transform.localScale;
        this.timer = 0f;
        this.stayTimer = 0f;
        this.started = true;
        this.stayStarted = false;
	}

	void Update () 
    {
        if (this.started)
        {
            timer += Time.deltaTime;
            float nt = timer / scaleTime;
            if (nt > 1)
            {
                started = false;
                //Destroy(gameObject, stayTime);
                if(this.destroyAfterStay)
                {
                    this.stayStarted = true;
                }
            }
            else
            {
                Vector3 currentScale = Vector3.Lerp(startScale, targetScale, nt);
                transform.localScale = currentScale;
            }
        }

        if(this.stayStarted)
        {
            this.stayTimer += Time.deltaTime;
            if(this.stayTimer >= this.stayTime )
            {
                this.stayStarted = false;
                Destroy(gameObject);
            }
        }
	}
}

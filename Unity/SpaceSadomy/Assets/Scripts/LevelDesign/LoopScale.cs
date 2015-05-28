using UnityEngine;
using System.Collections;

public class LoopScale : MonoBehaviour {

    public bool startAutomatically;
    public Vector3 startScale;
    public Vector3 endScale;
    public float scaleTime;
    public int numberIterations;

    private bool started;
    private int scaleCounter = 0;
    private float timer = 0f;

    void Start()
    {
        if(this.startAutomatically)
        {
            this.StartLoop();
        }
    }

    private void StartLoop()
    {
        this.scaleCounter = 0;
        this.ResetTimer();
        this.started = true;
    }

    void Update()
    {
        if(this.started)
        {
            bool completed;
            float nt = this.AdvanceTimer(out completed);
            transform.localScale = Vector3.Lerp(startScale, endScale, nt);

            if(completed)
            {
                this.transform.localScale = endScale;
                this.scaleCounter++;
                if(this.scaleCounter >= this.numberIterations)
                {
                    this.started = false;
                    Destroy(gameObject);
                }
                else
                {
                    this.ResetTimer();
                }
            }
        }
    }

    private float AdvanceTimer(out bool completed)
    {
        this.timer += Time.deltaTime;
        float nt =  timer / scaleTime;
        if(nt >= 1f )
        {
            completed = true;
        }
        else
        {
            completed = false;
        }
        return Mathf.Clamp01(nt);
    }

    private void ResetTimer()
    {
        this.timer = 0f;
    }
}

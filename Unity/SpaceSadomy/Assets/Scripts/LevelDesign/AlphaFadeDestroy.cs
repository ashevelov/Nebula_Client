using UnityEngine;
using System.Collections;

public class AlphaFadeDestroy : MonoBehaviour
{
    private bool started = false;
    private float interval = 0;

    private float startAlpha;
    private float endAlpha;

    private float timer = 0.0f;
    private float normalizedTimer;

    private Renderer cachedRenderer;

    public void DestroyObject(float interval)
    {
        //this.cachedRenderer = GetComponent<Renderer>();
        //this.interval = interval;
        //this.timer = 0.0f;
        //this.startAlpha = this.cachedRenderer.material.GetColor("_Color").a;
        //this.endAlpha = 0.0f;
        //this.normalizedTimer = 0.0f;
        //this.started = true;
        Destroy(gameObject);
    }

    /*
    void Start()
    {
        this.cachedRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (this.started)
        {
            this.timer += Time.deltaTime;
            this.normalizedTimer = this.timer / this.interval;

            if (this.normalizedTimer >= 1.0f)
            {
                this.started = false;
                Destroy(gameObject);
            }
            else
            {
                float alpha = Mathf.Lerp(this.startAlpha, this.endAlpha, this.normalizedTimer);
                this.SetMaterialAlpha(this.cachedRenderer.material, alpha);
            }
        }
    }


    private void SetMaterialAlpha(Material m, float alpha)
    {
        Color oldColor = m.GetColor("_Color");
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        m.SetColor("_Color", newColor);
    }*/
}

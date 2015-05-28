using UnityEngine;
using System.Collections;

public class TextureOffset : MonoBehaviour {

    public float xOffsetSpeed;
    public float yOffsetSpeed;

    private Vector2 currentOffset;
    private Renderer currentRenderer;

    void Start()
    {
        currentOffset = Vector2.zero;
        currentRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        currentOffset.x += xOffsetSpeed * Time.deltaTime;
        currentOffset.y += yOffsetSpeed * Time.deltaTime;
        if (currentOffset.x >= 1.0f)
            currentOffset.x -= 1.0f;
        if (currentOffset.x < 0)
            currentOffset.x += 1.0f;
        if (currentOffset.y >= 1.0f)
            currentOffset.y -= 1.0f;
        if (currentOffset.y < 0.0f)
            currentOffset.y += 1.0f;
        currentRenderer.material.SetTextureOffset("_MainTex", currentOffset);
    }
}

using UnityEngine;
using System.Collections;

public class WarpEffect : MonoBehaviour {

	private MeshRenderer meshRenderer;
    private Material mat;

	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
        mat = getMaterial;
	}
    public Vector4 offset = Vector4.zero;
    public Vector4 move;
    public float distortionMoveFactor;
	public float speed = 0.2f;

	void Update () {
        offset -= move * Time.deltaTime * speed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset.x, offset.y));
        mat.SetTextureOffset("_Offset", new Vector2(offset.x * distortionMoveFactor, offset.y * distortionMoveFactor)); 
	}

	private Material getMaterial
	{
		get
		{
			if (meshRenderer == null)
				return null;
			
			Material mat = meshRenderer.materials[0];
			if (mat == null)
				return null;
			
			return mat;
		}
	}
}

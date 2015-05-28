using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Aubergine/Buffers/RenderGlow")]
public class RenderGlow : MonoBehaviour {
	public Shader replacementShader;
	public Texture2D blackTexture;

	private RenderTexture renderTexture;
	private GameObject shaderCamera;
	
	void Awake () {
		Shader.SetGlobalTexture("_GlowTex", blackTexture);
	}

	void Start () {
		/*
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32)) {
			enabled = false;
			return;
		}
		*/
		if (!replacementShader || !replacementShader.isSupported) {
			enabled = false;
			return;
		}
	}

	void OnEnable () {
		replacementShader = Shader.Find("Aubergine/Buffers/RenderGlow");
	}

	void OnDisable () {
		DestroyImmediate(shaderCamera);
	}

	void OnPreCull () {
		if (!enabled || !gameObject.active) return;
		renderTexture = RenderTexture.GetTemporary((int)GetComponent<Camera>().pixelWidth, (int)GetComponent<Camera>().pixelHeight, 24, RenderTextureFormat.ARGB32);
		//renderTexture = RenderTexture.GetTemporary((int)camera.pixelWidth/8, (int)camera.pixelHeight/8, 24, RenderTextureFormat.ARGB32);
		if (!shaderCamera) {
			shaderCamera = new GameObject("Camera-GlowBuffer", typeof(Camera));
			shaderCamera.GetComponent<Camera>().enabled = false;
			shaderCamera.hideFlags = HideFlags.HideAndDontSave;
		}
		Camera cam = shaderCamera.GetComponent<Camera>();
		cam.CopyFrom(GetComponent<Camera>());
		cam.backgroundColor = new Color(0,0,0,1);
		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.targetTexture = renderTexture;
		cam.RenderWithShader(replacementShader, "RenderType");
		
		Shader.SetGlobalTexture("_GlobalGlowBufferTexture", renderTexture);
		Shader.SetGlobalVector("_GlobalGlowBufferTextureSize", new Vector4(renderTexture.width, renderTexture.height, 0, 0));
	}

	void OnPostRender () {
		if (!enabled || !gameObject.active) return;
		RenderTexture.ReleaseTemporary(renderTexture);
	}
}
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Aubergine/ImageFx/FX_Glow_SM2")]
public class PP_Glow_SM2 : PostProcessBase {
	//NO BLUR variable here as it is hard coded to the shader
	//This should be between 0 and 1
	public float glowStrength = 1f;

	void Awake () {
		base.material.SetFloat("_GlowStrength", glowStrength);
	}

	void OnEnable () {
		base.shader = Shader.Find("Hidden/Aubergine/ImageFx/FX_Glow_SM2");
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		base.material.SetFloat("_GlowStrength", glowStrength);
		Graphics.Blit (source, destination, base.material);
	}
}
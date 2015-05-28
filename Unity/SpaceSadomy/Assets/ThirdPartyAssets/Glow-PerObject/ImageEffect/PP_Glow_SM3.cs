using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Aubergine/ImageFx/FX_Glow_SM3")]
public class PP_Glow_SM3 : PostProcessBase {
	//This value represents the blur multiplier
	//A good value is in between 0 - 3(maybe 4)
	public float blurMultiplier = 2f;
	//This should be between 0 and 1
	public float glowStrength = 1f;

	void Awake () {
		base.material.SetFloat("_BlurMulti", blurMultiplier);
		base.material.SetFloat("_GlowStrength", glowStrength);
	}

	void OnEnable () {
		base.shader = Shader.Find("Hidden/Aubergine/ImageFx/FX_Glow_SM3");
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		base.material.SetFloat("_BlurMulti", blurMultiplier);
		base.material.SetFloat("_GlowStrength", glowStrength);
		Graphics.Blit (source, destination, base.material);
	}
}
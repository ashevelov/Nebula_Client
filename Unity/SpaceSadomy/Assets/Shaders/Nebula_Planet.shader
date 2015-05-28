// Simplified Bumped shader. Differences from regular Bumped one:
// - no Main Color
// - Normalmap uses Tiling/Offset of the Base texture
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Nebula/Planet" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_Emission("Emission Strength", Float) = 1
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 250

		CGPROGRAM
#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _Color;
		fixed _Emission;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = c.rgb * _Emission;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		}
		ENDCG
	}

	FallBack "Mobile/Diffuse"
}

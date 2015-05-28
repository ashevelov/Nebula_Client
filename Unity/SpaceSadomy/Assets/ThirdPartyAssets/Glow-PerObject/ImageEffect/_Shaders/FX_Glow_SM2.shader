Shader "Hidden/Aubergine/ImageFx/FX_Glow_SM2" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GlowStrength ("Glow Strength", float) = 1
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
		Pass {
			CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				
				sampler2D _MainTex;
				uniform float4 _MainTex_TexelSize;
				uniform sampler2D _GlobalGlowBufferTexture;
				float _GlowStrength;

				struct v2f {
					float4 pos : POSITION;
					half2 uv[2] : TEXCOORD0;
				};

				v2f vert( appdata_img v ) {
					v2f o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					half2 uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
					o.uv[0] = uv;
					//D3D Antialias fix
					#if SHADER_API_D3D9
					if (_MainTex_TexelSize.y < 0)
						uv.y = 1-uv.y;
					#endif
					o.uv[1] = uv;
					return o;
				}

				half4 frag (v2f i) : COLOR {
					half3 glo = tex2D(_GlobalGlowBufferTexture, i.uv[1]).rgb;
					//Simple one pass blur here, if you want to use.
					//Good for 2.0, if we want to do in 1 pass only
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]+0.001).rgb;
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]+0.002).rgb;
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]+0.003).rgb;
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]+0.004).rgb;
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]-0.001).rgb;
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]-0.002).rgb;
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]-0.003).rgb;
					glo += tex2D(_GlobalGlowBufferTexture, i.uv[1]-0.004).rgb;
					glo /= 2.5;

					//Return the color here
					half3 col = tex2D(_MainTex, i.uv[0]).rgb;
					half3 rCol = lerp(col, col+glo, _GlowStrength);
					return half4(rCol, 1);
					
					//return half4(glo, 1);
				}
			ENDCG
		}
	}
	Fallback off
}
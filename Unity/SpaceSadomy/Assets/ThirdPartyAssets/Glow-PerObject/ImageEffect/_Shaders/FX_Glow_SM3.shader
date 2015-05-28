Shader "Hidden/Aubergine/ImageFx/FX_Glow_SM3" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlurMulti ("Blur Multi", float) = 1
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
				#pragma target 3.0
				
				sampler2D _MainTex;
				uniform float4 _MainTex_TexelSize;
				uniform sampler2D _GlobalGlowBufferTexture;
				float _BlurMulti, _GlowStrength;
				
				static const float2 PixelKernelH[11] =
				{
					{ -5, 0 }, { -4, 0 }, { -3, 0 }, { -2, 0 }, { -1, 0 },
					{ 0,  0 },
					{ 1,  0 }, { 2,  0 }, { 3,  0 }, { 4,  0 }, { 5,  0 }
				};
				static const float2 PixelKernelV[11] =
				{
					{ 0, -5 }, { 0, -4 }, { 0, -3 }, { 0, -2 }, { 0, -1 },
					{ 0,  0 },
					{ 0,  1 }, { 0,  2 }, { 0,  3 }, { 0,  4 }, { 0,  5 }
				};
				
				static const float BlurWeights[11] = 
				{
					0.008764, 0.026995, 0.064759, 0.120985, 0.176033,
					0.199471,
					0.176033, 0.120985, 0.064759, 0.026995, 0.008764
				};
				
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
					//Kernel blur here, its 3.0 if we dont want to do 2 passes
					half3 glo = 0;
					//Horizontal blur
					for(int a = 0; a < 11; a++) {
						glo += tex2D(_GlobalGlowBufferTexture, i.uv[1] + PixelKernelH[a].xy * _BlurMulti / _ScreenParams.x) * BlurWeights[a];
					}
					//Vertical blur
					for(int a = 0; a < 11; a++) {
						glo += tex2D(_GlobalGlowBufferTexture, i.uv[1] + PixelKernelV[a].xy * _BlurMulti / _ScreenParams.y) * BlurWeights[a];
					}

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
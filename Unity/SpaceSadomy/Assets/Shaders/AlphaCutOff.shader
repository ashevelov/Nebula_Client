Shader "Space/AlphaCutOff" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Tex ("Base (RGB)", 2D) = "white" {}
		_CutOff("Cut Off", Range(0, 1)) = 0.5
		_EmptyColor("Empty Color", Color ) = (1, 0, 0, 1)
		_TintColor("Tint Color", Color ) = (1, 0, 0, 1)
	}
	SubShader {

			Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
		
		Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _Tex;
				float4 _MainTex_ST;
				float _CutOff;
				float4 _EmptyColor;
				float4 _TintColor;

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert( appdata_base v ) {
					v2f o;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex );
					return o;
				}

				float4 frag(v2f i ) : COLOR {
					float4 c = tex2D(_MainTex, i.uv);
					float4 c1 = tex2D(_Tex, i.uv);

					if( c.a < ( 1 -  _CutOff ) ) {
						c1.a = 0.0;
						//return c;
					} else {
						c1.a = _TintColor.a;
					}
					c1.rgb = c1.rgb * _TintColor.rgb;
					return c1;
				}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}

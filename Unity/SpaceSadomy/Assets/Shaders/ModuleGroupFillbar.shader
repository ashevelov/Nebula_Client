Shader "Space/Module Group Fillbar" {
	Properties {
		_MainTex("Main Texture", 2D ) = "white" {}
		_Color("Color", Color ) = (1, 1, 1, 1)
		_Value("Value", Range(0, 1)) = 0.4 
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Value;

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
					float4 result = tex2D(_MainTex, i.uv) * _Color;
					if(_Value < 1.0 ) {
						float dist = clamp( distance( i.uv, float2(0.5, 0.5)) / 0.5, 0, 1 );
						if( _Value == 0.0 )
							result.a = 0.1;
						else
						{
							if( dist > _Value )
							{
								float a = lerp( result.a * 0.5, 0.0, clamp ( dist , 0, 1) );
								result.a = a;
							}
							else
							{
								float a = lerp (result.a, result.a * 0.05, clamp ( dist , 0, 1) );
								result.a = a;
							}
						}

						return result;
					} else {
						return result;
					}
			}
			ENDCG
		}
	}
}

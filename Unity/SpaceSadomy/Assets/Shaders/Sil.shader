Shader "Space/Sil" {
	Properties {
		_Color("Color", Color) = (1, 1, 1, 0.5)
		_MainTex("Main Texture", 2D ) = "white"{}
		_Falloff("Falloff", Float) = 1.0
		_Radius("Radius", Range(0, 1.0)) = 0.5
	}

	SubShader {
		Tags { "Queue" = "Transparent" }
		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float4 _Color;
			uniform sampler2D _MainTex;
			uniform float _Falloff;
			uniform float _Radius;

			struct vertexInput {
				float4 vertex: POSITION;
				float3 normal: NORMAL;
				float4 texcoord: TEXCOORD0;
			};

			struct vertexOutput {
				float4 pos: SV_POSITION;
				float4 tex: TEXCOORD0;
				float3 normal: TEXCOORD1;
				float3 viewDir: TEXCOORD2;
			};

			vertexOutput vert(vertexInput input ) {
				vertexOutput output;
				float4x4 modelMatrix = _Object2World;
				float4x4 modelMatrixInverse = _World2Object;

				output.tex = input.texcoord;
				output.normal = normalize(float3(mul(float4(input.normal, 0.0), modelMatrixInverse)));
				output.viewDir = normalize(_WorldSpaceCameraPos - float3(mul(modelMatrix, input.vertex)));
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex );
				return output;
			}

			float4 frag(vertexOutput input) : COLOR {
				float3 normalDirection = normalize(input.normal);
				float3 viewDirection = normalize(input.viewDir);
				float4 c = tex2D(_MainTex, input.tex.xy);
				float d = dot(viewDirection, normalDirection);

				float coef = 1.0f;
				if( d > _Radius) 
				{
					float f = 1;
					if(_Falloff > 1 ) {
						f = _Falloff - 1;
					}
					coef = f * f * f;
				}
				else
				{
					coef = _Falloff;
				}
				float newOpacity = min( c.a, coef * ( 1.0 -  d) );
				return float4(c.rgb, newOpacity) * _Color;
			}
			ENDCG
		}
	}
}

Shader "Custom/ScreenSpot" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpotRadius("Spot radius", Float) = 0.02
		_SpotUV("Spot UV", Vector) = (0.5, 0.5, 0, 0)
		_SpotColor("Spot Color", Color ) = (1, 1, 1, 1)
		_Attenuation("Attenuation", Float) = 0.5
		_MinDistance("Min Distance", Float) = 100
		_MaxDistance("Max Distance", Float) = 10000
		_CurDistance("Current Distance", Float) = 1000
	}
	SubShader {
		Tags {"Queue" = "Transparent"} 
		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _SpotRadius;
			uniform float4 _SpotUV;
			uniform float4 _SpotColor;
			uniform float _Attenuation;
			uniform float _MinDistance;
			uniform float _MaxDistance;
			uniform float _CurDistance;


			struct vertexInput {
				float4 vertex: POSITION;
				float4 texcoord: TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos: SV_POSITION;
				float4 tex: TEXCOORD0;
			};

			vertexOutput vert(vertexInput input) {
				vertexOutput output;
				output.pos = mul( UNITY_MATRIX_MVP, input.vertex );
				output.tex = input.texcoord;
				return output;
			}

			float4 frag(vertexOutput input) : COLOR {
				float du = abs(_SpotUV.x - input.tex.x);
				float dv = abs(_SpotUV.y - input.tex.y);
				float dist2 = du * du + dv * dv;
				float r2 = _SpotRadius * _SpotRadius;
				float a = 0.0;

				if(_CurDistance >= _MinDistance && _CurDistance <= _MaxDistance)
				{
					if( dist2 <= r2 )
					{
						float startA = _SpotColor.a * ( abs(_CurDistance - _MaxDistance) / abs(_MaxDistance - _MinDistance) );
						a = startA -  (dist2 )/pow( r2, _Attenuation ) ;
						return float4(float3(_SpotColor), a);
					}
					else
					{
						return float4(0, 0, 0, 0);
					}
				}
				else
				{
					return float4(0, 0, 0, 0);
				}
			}
			ENDCG
		}
	} 

}

Shader "Space/FillbarCircle" {
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
				float vAx=1;
	            float vAy=0;
	            
	            float vBx=i.uv.x- 0.5;
	            float vBy=i.uv.y;
            
            
            	float cosL = (vAx*vBx+ vAy*vBy)  / (sqrt(vAx*vAx+vAy*vAy) * sqrt(vBx*vBx +vBy*vBy));
				//cosL = cos(cosL);
				
				if( cosL < _Value ) {
				
					float4 result = tex2D(_MainTex, i.uv) * _Color;
					if(_Value < 1.0 )
					 {
						float dist = _Value/cosL;
						float a = lerp( 0.0, result.a, clamp(dist, 0, 1) );
						result.a = a;
						return result;
					} else {
						return result;
					}
				}
				else
					return float4(1, 1, 1, 0);
			}
			ENDCG
		}
	}
}

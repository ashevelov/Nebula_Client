Shader "Space/EdgeShader" {
	Properties {
		_Color("Color", Color ) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		Pass {
			Cull Front
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vertexInput {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};
			uniform float4 _Color;

			vertexOutput vert(vertexInput IN ) {
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
				o.uv = IN.texcoord;
				return o;
			}
			float4 frag(vertexOutput IN ) : COLOR {
				if( IN.uv.x < 0.01 || IN.uv.x > 0.99 || IN.uv.y < 0.01 || IN.uv.y > 0.99 ) {
					return float4(1, 1, 1, 1) * _Color;
				} else {
					return float4(0, 0, 0, 0) * _Color;
				}
			}
			ENDCG
		}
		Pass {
			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vertexInput {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			uniform float4 _Color;

			vertexOutput vert(vertexInput IN ) {
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
				o.uv = IN.texcoord;
				return o;
			}
			float4 frag(vertexOutput IN ) : COLOR {
				if( IN.uv.x < 0.01 || IN.uv.x > 0.99 || IN.uv.y < 0.01 || IN.uv.y > 0.99 ) {
					return float4(1, 1, 1, 1) * _Color;
				} else {
					return float4(0, 0, 0, 0) * _Color;
				}
			}
			ENDCG
		}
	}
}

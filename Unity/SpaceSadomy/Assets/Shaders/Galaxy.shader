Shader "Space/Galaxy" {
	Properties {
		_MainTex("Main Texture", 2D) = "white"{}
		_Color("_Color", Color) = (1, 1, 1, 1)
	}

Category
{
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	Cull Off
	SubShader {
    	Pass {
        	Fog { Mode Off }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

				sampler2D _MainTex;
				float4 _Color;

			// vertex input: position, UV
			struct appdata {
    			float4 vertex : POSITION;
    			float4 texcoord : TEXCOORD0;
			};

			struct v2f {
    			float4 pos : SV_POSITION;
    			float4 uv : TEXCOORD0;
			};

			v2f vert (appdata v) {
					v2f o;
					o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
					o.uv = float4( v.texcoord.xy, 0, 0 );
					return o;
			}
			
			float4 frag( v2f i ) : COLOR {
					return tex2D( _MainTex, i.uv.xy ) * _Color;
			}
			ENDCG
    		}
		}
	}
} 
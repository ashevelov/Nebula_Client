Shader "Custom/Glow" {
	Properties {
		_GlowTexture("Glow Texture", 2D) = "white"{}
		_CenterPosition("Center Position", Vector) = (0, 0, 0, 0)
		_StarDirection("Star Direction", Vector) = (0, 0, 0, 0)
		_GlowFalloff("Glow Falloff", Float) = 1.0
		_MaxDepth("Max Depth", Float) = 0.5
	}

	SubShader {
		Pass {
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _GlowTexture;
			float4 _CenterPosition;
			float4 _StarDirection;
			float _GlowFalloff;
			float _MaxDepth;


			struct A2V
			{
				float4 vertex    : POSITION;
				float4 tangent   : TANGENT;
				float3 normal    : NORMAL;
				float2 texcoord  : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				fixed4 color     : COLOR;
			};
			struct V2F {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				fixed color : COLOR;
			};
			V2F vert(A2V i) {
				V2F o;
				float4 vertM = mul(_Object2World, i.vertex);
				float3 cam2vertM = _WorldSpaceCameraPos - vertM.xyz;
				float3 cam2vertMDir = normalize(cam2vertM);
				float3 localM = vertM.xyz - _CenterPosition;
				float3 localMDir = normalize(localM);
				float3 nearM = _WorldSpaceCameraPos - _CenterPosition;
				float3 nearMDir = normalize(nearM);
				float depth = length(localM - nearM);
				float scaledDepth = depth / _MaxDepth;
				float brightnessN = dot(_StarDirection.xyz, nearMDir);

				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex );
				o.texcoord.x = scaledDepth;
				o.texcoord.y = pow(scaledDepth, _GlowFalloff);
				o.color.x = brightnessN * 0.5f + 0.5f;
				return o;
			}

			float4 frag(V2F i ) : COLOR {
				float scaledDepth = i.texcoord.x;
				float opticalDepth = saturate(i.texcoord.y);
				float brightness = i.color.x;
				float4 glowColor = tex2D(_GlowTexture, float2(brightness, scaledDepth));
				glowColor.w *= opticalDepth;
				return glowColor;
			}
			ENDCG
		}
	}


}

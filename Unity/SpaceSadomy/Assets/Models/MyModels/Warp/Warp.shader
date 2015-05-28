Shader "Space/Warp"
{
	Properties 
	{
_MainTex("_MainTex", 2D) = "black" {}
_Gloss("_Gloss", Range(0,1) ) = 0.41133
_Specular("_Specular", Color) = (1,1,1,1)
_Distortion("_Distortion", Range(0,1) ) = 0.2068966
_Offset("_Offset", 2D) = "black" {}
_Dist("_Dist", Float) = 0
_Lerp("_Lerp", Float) = 0
_ScreenPos("_ScreenPos", Vector) = (0,0,0,0)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}
GrabPass { }
		
Cull Back
ZWrite Off
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


sampler2D _MainTex;
float _Gloss;
float4 _Specular;
float _Distortion;
sampler2D _Offset;
float _Dist;
float _Lerp;
float4 _ScreenPos;
sampler2D _GrabTexture;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, -s.Normal ));
				
				float nh = max (0, dot (-s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_MainTex;
float4 screenPos;
float2 uv_Offset;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D2=tex2D(_MainTex,(IN.uv_MainTex.xyxy).xy);
float4 Lerp0_1_NoInput = float4(0,0,0,0);
float4 Lerp0=lerp(Tex2D2,Lerp0_1_NoInput,_Lerp.xxxx);
float4 Add1=(float2(IN.screenPos.x/IN.screenPos.w, (-IN.screenPos.y)/IN.screenPos.w).xyxy) + _ScreenPos;
float4 Tex2D1=tex2D(_Offset,(IN.uv_Offset.xyxy).xy);
float4 UnpackNormal0=float4(UnpackNormal(Tex2D1).xyz, 1.0);
float4 Multiply1=UnpackNormal0 * _Dist.xxxx;
float4 Add0=Add1 + Multiply1;
float4 Tex2D0=tex2D(_GrabTexture,Add0.xy);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Lerp0;
o.Emission = Tex2D0;
o.Specular = _Gloss.xxxx;
o.Gloss = _Specular;
o.Alpha = Tex2D2.aaaa;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
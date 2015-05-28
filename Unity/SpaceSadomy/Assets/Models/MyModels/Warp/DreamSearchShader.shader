Shader "DreamShaders/DreamSearchShader"
{Properties {
   
    _MainTex("_MainTex", 2D) = "black" {}
	_EffectTexture("_EffectTexture", 2D) = "black" {}
	_EffectColor("_EffectColor", Color) = (1,1,1,1)
	_DissolveTex("_DissolveTex", 2D) = "black" {}
	_DistortionTex("_DistortionTex", 2D) = "black" {}
	_Distortion("_Distortion", Float) = 0
	_Lerp("_Lerp", Float) = 0
	_TextureDistortionPos("_TextureDistortionPos", Vector) = (0,0,0,0)
	_MainColor("_MainColor", Color) = (1,1,1,1)
      
                 }

Category {


    Tags {"Queue" = "Transparent"}
    Alphatest Greater 0
   
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Blend One OneMinusSrcAlpha
   //ColorMask RGB
   //Fog { Color [_AddFog] }
   SubShader {
   
   Pass {
//   Tags {"LightMode" = "Always"}
//   Blend SrcAlpha OneMinusSrcAlpha   

// 
   
////////////////////////////////////////AmbientPass/////////////////////////////////////////////
CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _EffectTexture;
			uniform float4 _EffectColor;
			uniform float _Lerp;
			uniform sampler2D _DissolveTex;
			uniform sampler2D _DistortionTex;
			uniform float _Distortion;
			uniform float4 _TextureDistortionPos;
			uniform float4x4 _Rotation;
			uniform float4 _MainColor;
			
			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv_MainTex : TEXCOORD0;
			    float2  uv_EffectTexture : TEXCOORD1;
			    float2  uv_DistortionTex : TEXCOORD2;
			    float2  uv_DissolveTex : TEXCOORD3;
			};
			
			float4 _MainTex_ST;
			float4 _EffectTexture_ST;
			float4 _DistortionTex_ST;
			
           v2f vert (appdata_base v)
			{
			    v2f o;
			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.uv_MainTex = TRANSFORM_TEX (v.texcoord, _MainTex);
			    o.uv_EffectTexture = TRANSFORM_TEX (v.texcoord, _EffectTexture);
			    o.uv_DistortionTex = TRANSFORM_TEX (v.texcoord, _DistortionTex);
			    o.uv_DissolveTex = TRANSFORM_TEX (v.texcoord, _DistortionTex);
			    return o;
			}
			half4 frag (v2f i) : COLOR
			{
				half4 UV_Pan0=float4((i.uv_DistortionTex.xyxy).x,(i.uv_DistortionTex.xyxy).y,(i.uv_DistortionTex.xyxy).z,(i.uv_DistortionTex.xyxy).w);
				half4 Tex2D3=tex2D(_DistortionTex,UV_Pan0.xy);
				half4 UnpackNormal0=float4(UnpackNormal(Tex2D3).xyz, 1.0);
				half4 Multiply0=UnpackNormal0 * _Distortion.xxxx;
				half4 Add3=_TextureDistortionPos + Multiply0;
				half4 Add4=(i.uv_MainTex.xyxy) + Add3;
				half4 Tex2D0=tex2D(_MainTex,Add4.xy) * _MainColor;
				
				//half4 MxV0=mul( _Rotation, (i.uv_EffectTexture.xyxy) );
				half4 Tex2D2=tex2D(_EffectTexture,i.uv_EffectTexture);//MxV0.xy);
				half4 Multiply1=_EffectColor * Tex2D2;
				half4 Min0=min(Tex2D0.aaaa,Multiply1);
				half4 Add1=Tex2D0 + Min0;
				half4 SplatAlpha0=_EffectColor.w;
				
				
				half4 Tex2D1=tex2D(_DissolveTex,(i.uv_DissolveTex.xyxy).xy);
				half4 Splat0=Tex2D1.x;
				half4 Subtract0=SplatAlpha0 - Splat0;

//				clip( Subtract0 );
				
			  //  half4 tex1 = tex2D (_MainTex, i.uv_MainTex);
			  //  half4 MxV0=mul( _Rotation, i.uv_EffectTexture.xyxy);
			 //   half4 tex2 = tex2D (_EffectTexture, MxV0.xy);
			 //   half4 tex = max(tex1, (min(tex2, tex1.a)));
			 
				half4 Lerp0=any(Subtract0 < 0)?half4(0,0,0,0):Add1;
			    return Lerp0;

				
//				half4 tex1 = tex2D(_MainTex, i.uv_MainTex);
				//float4 MxV0=mul( _Rotation, i.uv2);
				//float4 tex2	= min(tex2D(_EffectTexture, i.uv2), _EffectColor);
				//float4 tex = max(tex1, (min(tex2, tex1.a)));
				//return tex1;
			}
			ENDCG      
      
         SetTexture [_MainTex] {combine texture}
    }
   
   } 
}
FallBack "Diffuse", 1
}
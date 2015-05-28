Shader "Aubergine/Buffers/RenderGlow" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Base (RGB) Trans (A)", Color) = (1,1,1,1)
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
		_GlowTex ("Base (RGB) NoTrans", 2D) = "black" {}
	}

	Category {
		LOD 100
		Lighting Off
		Fog { Mode Off }

		SubShader {
			Tags {"RenderType" = "Opaque" "Queue" = "Geometry" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

					struct a2f {
						float4 vertex : POSITION;
						float4 texcoord : TEXCOORD0;
					};
					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
					};

					uniform sampler2D _MainTex;
					uniform float4 _MainTex_ST;
					uniform sampler2D _GlowTex;

					v2f vert(a2f v) {
						v2f o;
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "TransparentCutout" "Queue" = "AlphaTest" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

					struct a2f {
						float4 vertex : POSITION;
						float4 texcoord : TEXCOORD0;
					};
					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
					};

					uniform sampler2D _MainTex;
					uniform float4 _MainTex_ST;
					uniform fixed _Cutoff;
					uniform fixed4 _Color;
					uniform sampler2D _GlowTex;

					v2f vert(a2f v) {
						v2f o;
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 col = tex2D(_MainTex, i.Uv);
						clip(col.a * _Color.a - _Cutoff);
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "TreeBark" "Queue" = "Geometry" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma glsl_no_auto_normalization
					#include "UnityCG.cginc"
					#include "Lighting.cginc"
					#include "TerrainEngine.cginc"

					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
					};

					uniform sampler2D _MainTex;
					uniform float4 _MainTex_ST;
					uniform sampler2D _GlowTex;

					v2f vert(appdata_full v) {
						v2f o;
						TreeVertBark(v);
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "TreeLeaf" "Queue" = "AlphaTest" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma glsl_no_auto_normalization
					#include "UnityCG.cginc"
					#include "Lighting.cginc"
					#include "TerrainEngine.cginc"

					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
					};

					uniform sampler2D _MainTex;
					uniform fixed _Cutoff;
					uniform sampler2D _GlowTex;

					v2f vert(appdata_full v) {
						v2f o;
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = v.texcoord.xy;
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 col = tex2D(_MainTex, i.Uv);
						clip(col.a - _Cutoff);
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "TreeOpaque" "Queue" = "Geometry" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"
					#include "TerrainEngine.cginc"

					struct a2f {
						float4 vertex : POSITION;
						float4 texcoord : TEXCOORD0;
						fixed4 color : COLOR;
					};
					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
					};

					uniform sampler2D _MainTex;
					uniform float4 _MainTex_ST;
					uniform sampler2D _GlowTex;

					v2f vert(a2f v) {
						v2f o;
						TerrainAnimateTree(v.vertex, v.color.w);
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "TreeTransparentCutout" "Queue" = "AlphaTest" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"
					#include "TerrainEngine.cginc"

					struct a2f {
						float4 vertex : POSITION;
						float4 texcoord : TEXCOORD0;
						fixed4 color : COLOR;
					};
					struct v2f {
						float4 Pos : SV_POSITION;
						float2 Uv : TEXCOORD0;
					};

					uniform sampler2D _MainTex;
					uniform fixed _Cutoff;
					uniform sampler2D _GlowTex;

					v2f vert(a2f v) {
						v2f o;
						TerrainAnimateTree(v.vertex, v.color.w);
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = v.texcoord.xy;
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 col = tex2D(_MainTex, i.Uv);
						clip(col.a - _Cutoff);
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "TreeBillboard" "Queue" = "AlphaTest" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"
					#include "TerrainEngine.cginc"

					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
					};

					uniform sampler2D _MainTex;
					uniform fixed _Cutoff;
					uniform sampler2D _GlowTex;

					v2f vert(appdata_tree_billboard v) {
						v2f o;
						TerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv.x = v.texcoord.x;
						o.Uv.y = v.texcoord.y > 0;
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 col = tex2D(_MainTex, i.Uv);
						clip(col.a - 0.001);
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "GrassBillboard" "Queue" = "AlphaTest" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma glsl_no_auto_normalization
					#include "UnityCG.cginc"
					#include "TerrainEngine.cginc"

					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
						fixed4 Color : COLOR;
					};

					uniform sampler2D _MainTex;
					uniform fixed _Cutoff;
					uniform sampler2D _GlowTex;

					v2f vert(appdata_full v) {
						v2f o;
						WavingGrassBillboardVert(v);
						o.Color = v.color;
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = v.texcoord.xy;
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 col = tex2D(_MainTex, i.Uv);
						clip(col.a * i.Color.a - _Cutoff);
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}

		SubShader {
			Tags {"RenderType" = "Grass" "Queue" = "AlphaTest" "IgnoreProjector" = "True"}
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma glsl_no_auto_normalization
					#include "UnityCG.cginc"
					#include "TerrainEngine.cginc"

					struct v2f {
						float4 Pos : SV_POSITION;
						half2 Uv : TEXCOORD0;
						fixed4 Color : COLOR;
					};

					uniform sampler2D _MainTex;
					uniform fixed _Cutoff;
					uniform sampler2D _GlowTex;

					v2f vert(appdata_full v) {
						v2f o;
						WavingGrassVert(v);
						o.Color = v.color;
						o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
						o.Uv = v.texcoord.xy;
						return o;
					}
					fixed4 frag(v2f i) : COLOR {
						fixed4 col = tex2D(_MainTex, i.Uv);
						clip(col.a * i.Color.a - _Cutoff);
						fixed4 glo = tex2D(_GlowTex, i.Uv);
						return fixed4(glo.rgb, 1);
					}
				ENDCG
			}
		}
	}
	Fallback Off
}
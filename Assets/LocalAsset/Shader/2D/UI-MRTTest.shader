// 输入纹理
// 输出原色与灰色
Shader "UI/MRTTest"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			void frag(v2f i, out half4 color1 : COLOR0, out half4 color2 : COLOR1)
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				color1 = col;
				fixed f = (col.r + col.g + col.b) / 3;
				color2 = fixed4(f, f, f, 1);
			}
			ENDCG
		}
	}
}

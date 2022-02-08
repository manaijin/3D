Shader "3D/Custom4"
{
    Properties
    {
        _Diffuse("Diffuse", Color) = (1,1,1,1)
    }

        SubShader
    {
        Pass
        {
            CGPROGRAM
            #include "Lighting.cginc"

            fixed4 _Diffuse;
            sampler2D AfterAllTexture2;
            float4 AfterAllTexture2_ST;
            float4 AfterAllTexture2_TexelSize;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, AfterAllTexture2);
                o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
#if UNITY_UV_STARTS_AT_TOP
                if (AfterAllTexture2_TexelSize.y < 0.0)
                    o.uv.y = 1.0 - o.uv.y;
#endif
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Diffuse.xyz;
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 lambert = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
                fixed3 diffuse = lambert * _Diffuse.xyz * _LightColor0.xyz + ambient;
                fixed4 color = tex2D(AfterAllTexture2, i.uv);
                color.rgb = color.rgb * diffuse;
                return fixed4(color);
            }

            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
        FallBack "Diffuse"
}

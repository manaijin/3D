Shader "3D/Blender"
{
    Properties
    {
        _MainTex("Base 2D", 2D) = "white"{}
        _Diffuse("Diffuse", Color) = (1,1,1,1)        
        _CutOff("CutOff", float) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Opaque"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil{
                ref 1
                Comp NotEqual
                Pass Replace
            }

            CGPROGRAM
            #include "Lighting.cginc"

            float4 _MainTex_ST;
            fixed4 _Diffuse;
            sampler2D _MainTex;

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
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Diffuse.xyz;
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 lambert = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
                fixed3 diffuse = lambert * _Diffuse.xyz * _LightColor0.xyz + ambient;
                fixed4 color = tex2D(_MainTex, i.uv);
                color.rgb = color.rgb * diffuse;
                color.rgb = fixed3(1.0, 1.0, 1.0);
                color.a = _Diffuse.w;
                return color;
            }

            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
        FallBack "Diffuse"
}
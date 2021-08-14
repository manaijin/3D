Shader "UI/EdgeSobel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv[9] : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            // 单个像素尺寸大小
            half4 _MainTex_TexelSize;

            float _EdgeOnly;
            float4 _EdgeColor;
            float4 _BgColor;

            v2f vert (appdata_img v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                half2 uv = v.texcoord;
                
                o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
                o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, -1);
                o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, -1);
                o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
                o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);
                o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
                o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
                o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, 1);
                o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, 1);

                return o;
            }

            fixed luminance(fixed4 color){
                return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
            }

            half Sobel(v2f i){                
                const half Gx[9] =
                {
                    -1, -2, -1,
                    0,  0,  0,
                    1,  2,  1,
                };

                const half Gy[9] = 
                {
                    -1, 0,  1,
                    -2, 0,  2,
                    -1, 0,  1,
                };

                half texColor;
                half edgeX = 0;
                half edgeY = 0;

                for (int it = 0; it < 9; it++)
                {
                    texColor = luminance(tex2D(_MainTex, i.uv[it]));
                    edgeX += texColor * Gx[it];
                    edgeY += texColor * Gy[it];
                }

                return 1 - abs(edgeX) - abs(edgeY);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half edge = Sobel(i);
                if(edge > 0)
                {
                    return tex2D(_MainTex, i.uv[4]);
                    
                }else
                {
                    return fixed4(0,0,0,0);
                    
                }
            }
            ENDCG
        }
    }

    Fallback Off
}

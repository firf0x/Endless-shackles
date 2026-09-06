Shader "Hidden/PostEffects(DitheringEffect)"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherSize ("Dither Cell Size", Float) = 1.0
        _ColorLevels ("Color Levels per Channel", Range(0, 100)) = 8
        _PixelSize ("Pixelation Size", Range(0, 10)) = 0.0
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _DitherSize;
            float _ColorLevels;
            float _PixelSize;

            static const float bayer[16] = {
                0.0/16.0, 8.0/16.0, 2.0/16.0, 10.0/16.0,
                12.0/16.0, 4.0/16.0, 14.0/16.0, 6.0/16.0,
                3.0/16.0, 11.0/16.0, 1.0/16.0, 9.0/16.0,
                15.0/16.0, 7.0/16.0, 13.0/16.0, 5.0/16.0
            };

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Пикселизация (опционально)
                if (_PixelSize > 0.0)
                {
                    float2 pixel = floor(uv * _ScreenParams.xy / _PixelSize) * _PixelSize;
                    uv = pixel / _ScreenParams.xy;
                }

                fixed4 col = tex2D(_MainTex, uv);

                // Дизеринг
                float2 ditherCoord = uv * _ScreenParams.xy / max(_DitherSize, 0.001);
                int x = int(ditherCoord.x) % 4;
                int y = int(ditherCoord.y) % 4;
                float ditherValue = bayer[y * 4 + x];

                float levels = max(2.0, floor(_ColorLevels));
                float3 color = col.rgb;
                float3 quantized = floor(color * (levels - 1) + ditherValue) / (levels - 1);
                quantized = saturate(quantized);

                return fixed4(quantized, col.a);
            }
            ENDCG
        }
    }
}
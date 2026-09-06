Shader "Hidden/PostEffects(BoxBlurEffect)"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius (pixels)", Range(0, 5)) = 1.0
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
            float _BlurRadius;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 pixelSize = 1.0 / _ScreenParams.xy;
                int r = (int)floor(_BlurRadius);
                r = clamp(r, 0, 5); // ограничим для производительности

                if (r == 0) return tex2D(_MainTex, uv);

                float4 sum = 0;
                float count = 0;
                for (int dy = -r; dy <= r; dy++)
                {
                    for (int dx = -r; dx <= r; dx++)
                    {
                        float2 offset = float2(dx, dy) * pixelSize;
                        sum += tex2D(_MainTex, uv + offset);
                        count++;
                    }
                }
                return sum / count;
            }
            ENDCG
        }
    }
}
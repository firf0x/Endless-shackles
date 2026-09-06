Shader "Custom/PostEffects(vignette)"
{
    Properties
    {
        _MainTex    ("Texture", 2D)      = "white" {}
        _Intensity  ("Intensity", Range(0, 10)) = 0.5
        _Radius     ("Radius",   Range(0, 1)) = 0.5
        _Softness   ("Softness", Range(0, 10)) = 0.2
        _Color      ("Vignette Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float     _Intensity;
            float     _Radius;
            float     _Softness;
            float4    _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Координаты относительно центра с учётом соотношения сторон
                float2 uv = i.uv - 0.5;
                float aspect = _ScreenParams.x / _ScreenParams.y;
                uv.x *= aspect;

                // Нормализованное расстояние до угла
                float maxDist = length(float2(aspect * 0.5, 0.5));
                float dist    = length(uv) / maxDist;

                // Коэффициент виньетки: 1 – нет затемнения, 0 – максимальное
                float vignette = 1.0 - smoothstep(_Radius, _Radius + _Softness, dist);

                // Смешивание
                float3 finalColor = lerp(col.rgb, _Color.rgb, (1.0 - vignette) * _Intensity);

                return float4(finalColor, col.a);
            }
            ENDCG
        }
    }
}
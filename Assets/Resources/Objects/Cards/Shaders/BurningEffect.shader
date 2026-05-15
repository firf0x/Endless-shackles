Shader "Custom/Sprites/BurningEffect"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        
        // Параметры эффекта сгорания
        _BurnProgress ("Прогресс сгорания", Range(0, 1)) = 0
        _EdgeWidth ("Ширина кромки", Range(0, 0.3)) = 0.1
        _BurnColor ("Цвет огня", Color) = (1, 0.2, 0, 1)
        _EdgeGlowColor ("Цвет свечения", Color) = (1, 0.5, 0, 1)
        _GlowIntensity ("Интенсивность свечения", Range(0, 3)) = 1.5
        
        // Шум для маски сгорания
        _NoiseScale ("Масштаб шума", Range(1, 10)) = 4
        _NoiseOctaves ("Октавы", Range(1, 4)) = 3
        _NoiseSharpness ("Резкость", Range(0.5, 2)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha   // Premultiplied alpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

            // Свойства из блока Properties
            sampler2D _MainTex;
            fixed4 _Color;
            float _BurnProgress;
            float _EdgeWidth;
            fixed4 _BurnColor;
            fixed4 _EdgeGlowColor;
            float _GlowIntensity;
            float _NoiseScale;
            int _NoiseOctaves;
            float _NoiseSharpness;

            // ---------- Процедурный шум (независим от текстуры) ----------
            float random (float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }
            
            float smoothNoise (float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                float2 u = f * f * (3.0 - 2.0 * f);
                float a = random(i);
                float b = random(i + float2(1,0));
                float c = random(i + float2(0,1));
                float d = random(i + float2(1,1));
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }
            
            float fractalNoise (float2 uv, int octaves)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;
                for (int i = 0; i < octaves; i++)
                {
                    value += amplitude * smoothNoise(uv * frequency);
                    amplitude *= 0.5;
                    frequency *= 2.0;
                }
                return value;
            }
            // ------------------------------------------------

            v2f vert (appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                
                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                // 1. Основной цвет спрайта (текстура * вершинный цвет)
                fixed4 mainColor = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                // 2. Генерируем шум на основе UV спрайта (без изменений)
                float2 noiseUV = IN.texcoord * _NoiseScale;
                float noise = fractalNoise(noiseUV, _NoiseOctaves);
                noise = pow(noise, _NoiseSharpness);
                
                // 3. Отсекаем пиксели, которые "сгорели"
                clip(noise - _BurnProgress);
                
                // 4. Расчёт зоны кромки
                float edgeDist = noise - _BurnProgress;
                if (edgeDist < _EdgeWidth)
                {
                    // Смешиваем цвет текстуры и цвет огня
                    float t = edgeDist / _EdgeWidth;   // t=0 на границе, t=1 внутри
                    fixed4 fireColor = lerp(_EdgeGlowColor, _BurnColor, t);
                    fixed4 finalColor = lerp(mainColor, fireColor, 1.0 - t);
                    finalColor.rgb += _EdgeGlowColor.rgb * _GlowIntensity * (1.0 - t);
                    
                    // На кромке делаем полностью непрозрачным (альфа = 1)
                    finalColor.a = 1.0;
                    
                    // Premultiply alpha
                    finalColor.rgb *= finalColor.a;
                    return finalColor;
                }
                else
                {
                    // Обычная область – сохраняем исходную прозрачность
                    mainColor.rgb *= mainColor.a;
                    return mainColor;
                }
            }
            ENDCG
        }
    }
    Fallback "Sprites/Default"
}
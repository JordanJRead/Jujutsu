Shader "Unlit/Domain"
{
    Properties
    {
        _T ("T", Float) = 0.5
        _InitialNoiseAmplitude ("Initial Noise Amplitude", Float) = 0.1
        _InitialNoiseScale ("Initial Noise Scale", Float) = 7
        _NoiseAmplitudeMult ("Noise Amplitude Mult", Float) = 0.5
        _NoiseScaleMult ("Noise Scale Mult", Float) = 3
        _NoiseOctaveCount ("Noise Octave Count", Float) = 5
        _Brightness ("Brightness", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "noiseSimplex.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 pos : TEXCOORD0;
            };

            float _T;
            float _InitialNoiseAmplitude;
            float _NoiseAmplitudeMult;
            float _InitialNoiseScale;
            float _NoiseScaleMult;
            int _NoiseOctaveCount;
            float _Brightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float amplitude = _InitialNoiseAmplitude;
                float scale = _InitialNoiseScale;
                float randOffset = 0;

                for (int j = 0; j < _NoiseOctaveCount; ++j) {
                    randOffset += (snoise(i.pos * scale) / 2 + 0.5) * amplitude;
                    amplitude *= _NoiseAmplitudeMult;
                    scale *= _NoiseScaleMult;
                }

                fixed4 col = fixed4(i.pos, 1);
                if (dot(i.pos.xyz, float3(0, 0, 1)) / 2 + 0.5 + randOffset < 1 - _T) {
                    clip(-1);
                }
                float brightness = _Brightness * (1 - _T);
                return fixed4(randOffset * brightness, randOffset * brightness, randOffset * brightness, 1);
            }
            ENDCG
        }
    }
}

Shader "Hidden/Kuwahara_URP_Simple"
{
    Properties
    {
        _KernelSize ("Kernel Size (Brush Size)", Range(2, 20)) = 10
        _Sharpness ("Sharpness", Float) = 8.0
        _Hardness ("Hardness (Daub Shape)", Float) = 4.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off

        Pass
        {
            Name "KuwaharaPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            // --- THE STEERING WHEEL VARIABLES ---
            int _KernelSize;
            float _Sharpness;
            float _Hardness;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                float n = float((_KernelSize + 1) * (_KernelSize + 1));
                
                // Arrays to hold the 4 quadrants (mean and variance)
                float3 m[4];
                float3 s[4];

                // Initialize arrays
                for (int k = 0; k < 4; ++k) {
                    m[k] = 0.0;
                    s[k] = 0.0;
                }

                // --- LOOP 1: Top Left ---
                for (int j = -_KernelSize; j <= 0; ++j)  {
                    for (int i = -_KernelSize; i <= 0; ++i)  {
                        float3 c = SampleSceneColor(uv + float2(i, j) * _ScreenParams.zw).rgb;
                        m[0] += c;
                        s[0] += c * c;
                    }
                }

                // --- LOOP 2: Top Right ---
                for (int j = -_KernelSize; j <= 0; ++j)  {
                    for (int i = 0; i <= _KernelSize; ++i)  {
                        float3 c = SampleSceneColor(uv + float2(i, j) * _ScreenParams.zw).rgb;
                        m[1] += c;
                        s[1] += c * c;
                    }
                }

                // --- LOOP 3: Bottom Right ---
                for (int j = 0; j <= _KernelSize; ++j)  {
                    for (int i = 0; i <= _KernelSize; ++i)  {
                        float3 c = SampleSceneColor(uv + float2(i, j) * _ScreenParams.zw).rgb;
                        m[2] += c;
                        s[2] += c * c;
                    }
                }

                // --- LOOP 4: Bottom Left ---
                for (int j = 0; j <= _KernelSize; ++j)  {
                    for (int i = -_KernelSize; i <= 0; ++i)  {
                        float3 c = SampleSceneColor(uv + float2(i, j) * _ScreenParams.zw).rgb;
                        m[3] += c;
                        s[3] += c * c;
                    }
                }

                float min_sigma2 = 1e+2;
                float3 col = 0;
                float weight_sum = 0.0;

                // --- COMPARE AND BLEND ---
                for (int k = 0; k < 4; ++k) {
                    m[k] /= n;
                    s[k] = abs(s[k] / n - m[k] * m[k]);
                    
                    float sigma2 = s[k].r + s[k].g + s[k].b;
                    
                    // The "Hardness" math determines how much we blend vs picking the best one
                    float w = 1.0 / (1.0 + pow(abs(_Hardness * 1000.0 * sigma2), 0.5 * _Sharpness));
                    
                    col += m[k] * w;
                    weight_sum += w;
                }

                return float4(1.0, 0.0, 0.0, 1.0); // Force RED
            }
            ENDHLSL
        }
    }
}
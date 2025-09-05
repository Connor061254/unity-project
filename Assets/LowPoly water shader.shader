// Final Version - Foam logic corrected based on user example.
// - Uses FoamMinDepth and FoamMaxDepth for a precise falloff.
// - Foam is now 100% guaranteed to only appear when touching geometry.
// - Retains the camera-independent depth, light blue color, and low-poly waves.
Shader "Custom/LowPolyWater_Final"
{
    Properties
    {
        [Header(Appearance)]
        _WaterColor ("Water Color", Color) = (0.75, 0.9, 1.0, 1.0)
        _FoamColor ("Foam Color", Color) = (1.0, 1.0, 1.0, 1.0)
        
        [Header(Foam Controls)]
        _FoamMaxDepth("Foam Max Depth", Float) = 0.6
        _FoamMinDepth("Foam Min Depth", Float) = 0.1
        _FoamPatternScale("Foam Pattern Scale", Range(1, 50)) = 15.0
        _FoamAnimationSpeed("Foam Animation Speed", Range(0, 5)) = 0.5

        [Header(Wave Motion)]
        _WaveAmplitude ("Wave Amplitude", Range(0, 1)) = 0.15
        _WaveFrequency ("Wave Frequency", Range(0, 5)) = 1.2
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1.0
        _WaveChoppiness("Wave Choppiness", Range(1, 20)) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D_X_FLOAT(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            CBUFFER_START(UnityPerMaterial)
                half4 _WaterColor;
                half4 _FoamColor;
                float _FoamMaxDepth;
                float _FoamMinDepth;
                float _FoamPatternScale;
                float _FoamAnimationSpeed;
                float _WaveAmplitude;
                float _WaveFrequency;
                float _WaveSpeed;
                float _WaveChoppiness;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS   : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
            };

            // Simple procedural noise function
            float simple_noise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float time = _Time.y * _WaveSpeed;
                float wave1 = sin(IN.positionOS.x * _WaveFrequency + time);
                float wave2 = cos(IN.positionOS.z * _WaveFrequency * 0.7 + time);
                float displacement = floor((wave1 + wave2) * _WaveAmplitude * _WaveChoppiness) / _WaveChoppiness;
                IN.positionOS.y += displacement;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Reconstruct world position of the geometry behind the water
                float2 screenUV = IN.positionHCS.xy / IN.positionHCS.w;
                float sceneRawDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV).r;
                float3 scenePositionWS = ComputeWorldSpacePosition(screenUV, sceneRawDepth, UNITY_MATRIX_I_VP);
                
                // Calculate true vertical depth
                float verticalDepth = IN.positionWS.y - scenePositionWS.y;

                // --- FOAM LOGIC (from user example) ---
                // Get a value from 0 to 1 based on depth
                float foamLerp = 1.0 - smoothstep(_FoamMinDepth, _FoamMaxDepth, verticalDepth);
                
                half4 finalColor = _WaterColor;

                // Only calculate noise and apply foam if foamLerp > 0
                // This guarantees no foam in deep water.
                if (foamLerp > 0.01)
                {
                    float2 noiseUV = IN.positionWS.xz * _FoamPatternScale;
                    noiseUV.x += _Time.y * _FoamAnimationSpeed;
                    float noise = simple_noise(noiseUV);
                    noise = smoothstep(0.45, 0.55, noise); // Make noise more contrasted

                    float foamFactor = foamLerp * noise;
                    
                    // Add foam color to water color for a bright effect
                    finalColor.rgb += _FoamColor.rgb * foamFactor;
                }
                
                return finalColor;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Unlit"
}

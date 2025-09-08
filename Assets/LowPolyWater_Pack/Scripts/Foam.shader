Shader "Unlit/WaterWithFoam"
{
    Properties
    {
        _WaterColor("Water Color", Color) = (0.0, 0.4, 0.6, 1.0)
        _FoamColor("Foam Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _FoamMaxDepth("Foam Max Depth (End Fade)", Float) = 0.5
        _FoamMinDepth("Foam Min Depth (Start Fade)", Float) = 0.2
    }
    SubShader
    {
        // This is an opaque shader
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            // Access to the camera's depth information
            sampler2D _CameraDepthTexture;

            // Properties from the inspector
            fixed4 _WaterColor;
            fixed4 _FoamColor;
            float _FoamMaxDepth;
            float _FoamMinDepth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // --- 1. Calculate Water Depth ---
                float sceneRawDepth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
                float sceneEyeDepth = LinearEyeDepth(sceneRawDepth);
                float waterEyeDepth = i.screenPos.w;
                float waterDepth = sceneEyeDepth - waterEyeDepth;

                // --- 2. FOAM SYSTEM ---
                // This is the core of the foam effect.
                // It creates a smooth gradient from 0 to 1 between the min and max depth.
                float foamLerp = smoothstep(_FoamMinDepth, _FoamMaxDepth, waterDepth);
                // We invert the value so that it's 1 (full foam) in shallow water
                // and 0 (no foam) in deep water.
                foamLerp = 1.0 - foamLerp;
                // --- END OF FOAM SYSTEM ---

                // --- 3. Combine Colors ---
                // We use lerp (linear interpolation) to mix the two colors.
                // If foamLerp is 0, we get 100% _WaterColor.
                // If foamLerp is 1, we get 100% _FoamColor.
                // Anything in between gives a smooth blend.
                fixed4 finalColor = lerp(_WaterColor, _FoamColor, foamLerp);

                return finalColor;
            }
            ENDCG
        }
    }
}
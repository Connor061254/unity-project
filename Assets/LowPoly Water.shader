Shader "Unlit/LowPolyWaterShader"
{
    Properties
    {
        _DeepColor("Deep Water Color", Color) = (0.0, 0.2, 0.4, 0.8)
        _ShallowColor("Shallow Water Color", Color) = (0.0, 0.6, 0.8, 0.8)
        _DepthMaxDistance("Depth Max Distance", Float) = 5

        _FoamColor("Foam Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _FoamMaxDepth("Foam Max Depth", Float) = 0.5
        _FoamMinDepth("Foam Min Depth", Float) = 0.2
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

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
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            sampler2D _CameraDepthTexture;

            fixed4 _DeepColor;
            fixed4 _ShallowColor;
            float _DepthMaxDistance;

            fixed4 _FoamColor;
            float _FoamMaxDepth;
            float _FoamMinDepth;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Get the screen position of the vertex
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate depth
                float sceneRawDepth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
                float sceneEyeDepth = LinearEyeDepth(sceneRawDepth);
                float waterEyeDepth = i.screenPos.w;
                float waterDepth = sceneEyeDepth - waterEyeDepth;

                // Calculate color based on depth
                float depthLerp = saturate(waterDepth / _DepthMaxDistance);
                fixed4 waterColor = lerp(_ShallowColor, _DeepColor, depthLerp);

                // Calculate foam based on depth
                float foamLerp = smoothstep(_FoamMinDepth, _FoamMaxDepth, waterDepth);
                foamLerp = 1.0 - foamLerp; // Invert to get foam in shallow areas
                fixed4 foam = foamLerp * _FoamColor;

                // Combine water color and foam
                fixed4 finalColor = waterColor + foam;
                finalColor.a = waterColor.a; // Use the water's alpha

                return finalColor;
            }
            ENDCG
        }
    }
}
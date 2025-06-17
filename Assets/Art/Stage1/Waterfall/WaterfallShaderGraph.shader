
Shader "URP/WaterfallSimple"
{
    Properties
    {
        _BaseMap("Water Texture", 2D) = "white" {}
        _BaseColor("Tint", Color) = (0.3, 0.7, 1, 1)
        _ScrollSpeed("Scroll Speed", Float) = 0.2
        _Alpha("Alpha", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float _ScrollSpeed;
            float _Alpha;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float2 uv = IN.uv + float2(0, _Time.y * _ScrollSpeed);
                OUT.uv = TRANSFORM_TEX(uv, _BaseMap);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                col.a *= _Alpha;
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}

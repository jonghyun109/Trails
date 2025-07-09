Shader "Custom/ShadowOnly"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
        }
    }
}
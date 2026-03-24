Shader "Custom/OcclusionShader"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry-1"
        }

        ZWrite On
        ZTest LEqual
        ColorMask 0

        Pass { }
    }
}
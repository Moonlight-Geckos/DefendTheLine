Shader "Custom/RectangleGrid" {
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _LineColor("LineColor", Color) = (1,1,1,1)
        _LineWidth("Line Width", Float) = 1
        _LineOffset("Line Offset", Float) = 1
        _GridWidth("Grid Width", Range(0,10)) = 4
        _GridHeight("Grid Height", Range(0,10)) = 4
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 200

        CGPROGRAM
            #pragma surface surf Standard fullforwardshadows vertex:vert

        #pragma target 3.0

        sampler2D _MainTex;

        fixed4 _Color;
        fixed4 _LineColor;
        float _LineWidth;
        float _LineOffset;
        int _GridWidth;
        int _GridHeight;


        struct Input
        {
            float4 normal;
            float2 uv_MainTex;
        };

        float Unity_Rectangle_float(float2 UV, float Width, float Height)
        {
            float2 d = abs(UV * 2 - 1) - float2(Width, Height);
            d = 1 - d / fwidth(d);
            return saturate(min(d.x, d.y));
        }
        float2 Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset)
        {
            return UV * Tiling + Offset;
        }
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
        }

        void surf(Input i, inout SurfaceOutputStandard o)
        {
            float2 uvs = frac(Unity_TilingAndOffset_float(i.uv_MainTex, float2(_GridWidth, _GridHeight), _LineOffset)) ;
            float rec = 1 - Unity_Rectangle_float(uvs, _LineWidth, _LineWidth);
            fixed4 col = lerp(_Color, _LineColor, rec);
            o.Albedo = col;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
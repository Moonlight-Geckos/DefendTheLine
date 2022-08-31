Shader "Custom/RectangleGrid" {
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _LineColor("LineColor", Color) = (1,1,1,1)
        _LineWidth("Line Width", Float) = 1
        _Radius("Border Radius", Float) = 1
        _GridWidth("Grid Width", Range(0,10)) = 4
        _GridHeight("Grid Height", Range(0,10)) = 4
    }
    SubShader
    {
        Pass
        {
            Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
            LOD 200

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 3.0

            fixed4 _Color;
            fixed4 _LineColor;
            float _LineWidth;
            float _Radius;
            int _GridWidth;
            int _GridHeight;


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                uint instanceID : SV_InstanceID;
            };

            struct Input
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv_MainTex : TEXCOORD0;
            };

            float Unity_Rectangle_float(float2 UV, float Width, float Radius)
            {

                return length(max(abs(UV * 2 - 1) - Width + Radius, 0.0)) - Radius;
            }
            float2 Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset)
            {
                return UV * Tiling + Offset;
            }
            Input vert(appdata v) 
            {

                Input output;
                output.normal = v.normal;
                output.uv_MainTex = v.uv;
                output.vertex = UnityObjectToClipPos(v.vertex);

                return output;
            }
            float4 frag(Input i) : SV_Target
            {
                float2 uvs = frac(Unity_TilingAndOffset_float(i.uv_MainTex, float2(_GridWidth, _GridHeight), 0));
                float rec = 1 - Unity_Rectangle_float(uvs, _LineWidth, _Radius);
                float4 col;
                if (rec > 0)
                    col = _LineColor;
                else
                    col = _Color;
                return col;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
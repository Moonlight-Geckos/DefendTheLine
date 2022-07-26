Shader "Custom/OptimizedColor" {
    Properties{
        _Color("Color", Color) = (1.0,1.0,1.0,1.0)
    }
        SubShader{
            Tags
        {
            "RenderType" = "Opaque"
        }
            LOD 150

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd

        float4 _Color;

        struct Input {
            half color : COLOR;
        };
        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = _Color;
        }
        ENDCG
    }
        FallBack "Diffuse"  }
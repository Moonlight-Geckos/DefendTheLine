Shader "Custom/OptimizedColor" {
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode" = "Vertex" }
            Lighting Off
            SetTexture[_MainTex] {
                constantColor(1,1,1,1)
                combine texture, constant
            }
        }

        Pass
        {
            Tags{ "LIGHTMODE" = "VertexLM" "RenderType" = "Opaque" }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"
            #pragma multi_compile_fog
            #define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

            // uniforms
            float4 _MainTex_ST;
            float4 _Color;

            // vertex shader input data
            struct appdata
            {
                float3 pos : POSITION;
                float3 uv1 : TEXCOORD1;
                float3 uv0 : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            // vertex-to-fragment interpolators
            struct v2f
            {
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 pos : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata IN)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.uv0 = IN.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                o.uv1 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;

                o.pos = UnityObjectToClipPos(IN.pos);
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 col, tex;

                half4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.uv0.xy);
                col.rgb = DecodeLightmap(bakedColorTex);

                tex = tex2D(_MainTex, IN.uv1.xy) * _Color;
                col.rgb = tex.rgb * col.rgb;
                col.a = 1;

                return col;
            }
            ENDCG
        }
    }
}
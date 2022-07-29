Shader "Custom/GrassShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
        _MainTex ("Main Texture", 2D) = "white" {}
        _Scale("Scale", Range(0,10)) = 1
        _WaveSpeed("Wave Speed", Float) = 1.0
        _WaveAmp("Wave Amp", Float) = 1.0
        _WindSpeed("Wind Speed", Float) = (1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque"}
        ZWrite On
        Cull Off
        LOD 100

        Pass{

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag alphatest:_Cutoff
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _Cutoff;
            CBUFFER_END


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                uint instanceID : SV_InstanceID;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float3 normal : NORMAL;
            };

            float3 _WindSpeed;
            float _WaveSpeed;
            float _WaveAmp;
            float _Scale;

            fixed4 _Color;

            StructuredBuffer<float3> _positionsBuffer;


            inline float unity_noise_randomValue(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            inline float unity_noise_interpolate(float a, float b, float t)
            {
                return (1.0 - t) * a + (t * b);
            }

            inline float unity_valueNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            inline float Unity_SimpleNoise_float(float2 UV, float Scale)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3 - 0));
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3 - 1));
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3 - 2));
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                return t;
            }

            float3 Unity_RotateAboutAxis_Radians_float(float3 In, float3 Axis, float Rotation)
            {
                float3 Out;
                float s = sin(Rotation);
                float c = cos(Rotation);
                float one_minus_c = 1.0 - c;

                Axis = normalize(Axis);
                float3x3 rot_mat =
                { one_minus_c * Axis.x * Axis.x + c, one_minus_c * Axis.x * Axis.y - Axis.z * s, one_minus_c * Axis.z * Axis.x + Axis.y * s,
                    one_minus_c * Axis.x * Axis.y + Axis.z * s, one_minus_c * Axis.y * Axis.y + c, one_minus_c * Axis.y * Axis.z - Axis.x * s,
                    one_minus_c * Axis.z * Axis.x - Axis.y * s, one_minus_c * Axis.y * Axis.z + Axis.x * s, one_minus_c * Axis.z * Axis.z + c
                };
                Out = mul(rot_mat, In);
                return Out;
            }

            v2f vert(appdata input) {
                v2f output;

                float j = Unity_SimpleNoise_float(float2(input.instanceID, input.instanceID), 8);

                _WaveSpeed += sin(j) / 4;
                _WaveAmp += cos(j) / 4;

                input.vertex.xyz = Unity_RotateAboutAxis_Radians_float(input.vertex.xyz, float3(0,1,0), j % 360);

                float4 data = float4(_positionsBuffer[input.instanceID], 0);

                input.vertex.y  *= (sin(j) + 1) * _Scale;

                input.vertex.y -= (sin(j)) * 0.5f;

                output.normal = input.normal;
                output.uv_MainTex = input.uv;

                float windSample = _Time.y * _WindSpeed.xz ;


                input.vertex.z += sin(_WaveSpeed * windSample * j) * _WaveAmp * pow(input.uv.y, 2);
                input.vertex.x += cos(_WaveSpeed * windSample * j) * _WaveAmp * pow(input.uv.y, 2);

                output.vertex = UnityObjectToClipPos(input.vertex + data);


                return output;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

                clip(c.a - _Cutoff);
                return c;
            }
            ENDCG
        }
    }
}

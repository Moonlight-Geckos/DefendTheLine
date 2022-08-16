// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SlimeShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}

        _NoiseScale("Noise Scale", Range(0,30)) = 1
        _NoiseSpeed("Noise Speed", Range(0,10)) = 0.5
        _WaveSize("Wave Size", Range(0,10)) = 0.5
        _RandomNum("Random Number", Range(-10,10)) = 0

        _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
        _FresnelPower("Fresnel Power", Range(0,10)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            sampler2D _MainTex;
            half _FresnelPower;
            half _NoiseScale;
            half _NoiseSpeed;
            half _RandomNum;
            half _WaveSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 tan : TANGENT;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };


            fixed4 _Color;
            fixed4 _FresnelColor;

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

            float Unity_SimpleNoise_float(float2 UV, float Scale)
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
            float2 Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax)
            {
                float2 Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                return Out;
            }
            float Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power)
            {
                float2 Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
                return Out;
            }
            v2f vert(appdata v)
            {
                v2f o;

                float3 v0 = v.vertex.xyz;
                float3 v1 = v0 + (v.tan.xyz * _WaveSize);

                float3 bitangent = cross(v.normal, v.tan.xyz);
                float3 v2 = v0 + (bitangent * _WaveSize);

                float noise0 = Unity_SimpleNoise_float(v0 + (_Time.y * _NoiseSpeed) + _RandomNum, _NoiseScale);
                float noise1 = Unity_SimpleNoise_float(v1 + (_Time.y * _NoiseSpeed) + _RandomNum, _NoiseScale);
                float noise2 = Unity_SimpleNoise_float(v2 + (_Time.y * _NoiseSpeed) + _RandomNum, _NoiseScale);

                v0.xyz += ((noise0 + 1) / 2) * v.normal;

                v1.xyz += ((noise1 + 1) / 2) * v.normal;

                v2.xyz += ((noise2 + 1) / 2) * v.normal;

                float3 vn = cross(v2 - v0, v1 - v0);

                v.normal = normalize(-vn);
                v.vertex.xyz = v0 * .5;
                v.normal.xyz = v0 * .5;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.normal = normalize(mul(unity_ObjectToWorld, v.normal));
                o.uv_MainTex = v.uv;

                /*
                float2 pp = (mul(unity_ObjectToWorld, v.normal)).xz + (_Time * (_NoiseSpeed + _RandomNum));
                float noise = Unity_SimpleNoise_float(pp, _NoiseScale);
                float remNoise = Unity_Remap_float2(noise, float2(0, 1), float2(-1, 1)) * _WaveSize;
                float remNoise2 = Unity_Remap_float2(noise, float2(-1, 1), float2(0, 1)) * _WaveSize;
                v.vertex.xz += remNoise;
                v.normal.xyz *= remNoise2;
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.normal = normalize(mul(unity_ObjectToWorld, v.normal));
                o.uv_MainTex = v.uv;

                o.vertex = UnityObjectToClipPos(v.vertex);
                */
                return o;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                float3 fres = Unity_FresnelEffect_float(IN.normal, IN.viewDir, _FresnelPower);
                col.rgb = lerp(col.rgb, _FresnelColor.rgb, fres);

                return col;
            }

            ENDCG
        }
    }

}

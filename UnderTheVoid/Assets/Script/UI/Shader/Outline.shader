Shader "CustomLighting/Outline"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("BaseColor Texture", 2D) = "white" {}

        [MaterialEnum(Off,0,Front,1,Back,2)] _Cull("Cull", Int) = 2

        [Header(Outline)]
        _OutlineColor("Outline Color", Color) = (0,0,0,0)
        _OutlineWidth("Outline Width", Range(0,10)) = 1
        [Header(Normal)]
        _BumpMap("Normal Texture",2D) = "bump"{}
        _NorMalStrength("Normal Strength",float) = 1

    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "SimpleLit"
            "RenderType" = "Opaque"
            "Queue" = "Geometry+0"
        }
        LOD 100
        Cull[_Cull]

        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            //ZTest LEqual


            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile_fog

            // Recieve Shadow
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT


            CBUFFER_START(UnityPerMaterial)

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_ST;
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); 
            half4 _Color;
            float _NorMalStrength;

            float4 _OutlineColor;
            float _OutlineWidth;

            CBUFFER_END




            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 lightmapUV : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 lightmapUV : TEXCOORD1;
                float fogCoord  : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3;
                float3 tangent : TEXCOORD4;
                float3 biTangent : TEXCOORD5;
                float3 wolrdPos : TEXCOORD6;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.lightmapUV = v.lightmapUV;

                o.normal = TransformObjectToWorldNormal(v.normal);
                o.tangent = TransformObjectToWorldDir(v.tangent.xyz);
                o.biTangent = cross(o.normal, o.tangent) * v.tangent.w;

                o.fogCoord = ComputeFogFactor(o.vertex.z);
                //#ifdef _MAIN_LIGHT_SHADOWS
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.shadowCoord = GetShadowCoord(vertexInput);
                //#endif
                

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {

                UNITY_SETUP_INSTANCE_ID(i);

                Light mainLight = GetMainLight(i.shadowCoord);

                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap,sampler_BumpMap,i.texcoord)) * float3(_NorMalStrength,_NorMalStrength,1);
                float3x3 tangentToWorld = float3x3(i.tangent,i.biTangent,i.normal);
                float3 normalWS = normalize(mul(normalTS,tangentToWorld));

                float2 uv = i.texcoord.xy * (_MainTex_ST.xy + _MainTex_ST.zw);
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * _Color;
                float ndotl = max(0, dot(normalize(mainLight.direction), normalWS));

                half3 ambient = SampleSH(i.normal);

                float4 diffus;
                diffus.rgb = albedo.rgb;
                diffus.a = albedo.a;

                diffus.rgb *= mainLight.color.rgb * (ndotl * mainLight.shadowAttenuation + ambient);

               // float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord) * _Color;
                diffus.rgb = MixFog(diffus.rgb, i.fogCoord);
                return diffus;
            }

            ENDHLSL
        }
        Pass
        {
             Name "ShadowCaster"

                Tags{"LightMode" = "ShadowCaster"}

                Cull[_Cull]

                HLSLPROGRAM

                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0

                #pragma vertex ShadowPassVertex
                #pragma fragment ShadowPassFragment

                // GPU Instancing
                 #pragma multi_compile_instancing
                
//#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/Shaders/SHadowCasterPass.hlsl"
            

                 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                 #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

        
                 CBUFFER_START(UnityPerMaterial)
                 CBUFFER_END

                 struct VertexInput
                 {
                     float4 vertex : POSITION;
                     float4 normal : NORMAL;

                     UNITY_VERTEX_INPUT_INSTANCE_ID
                 };

                 struct VertexOutput
                 {
                     float4 vertex : SV_POSITION;

                     UNITY_VERTEX_INPUT_INSTANCE_ID
                     UNITY_VERTEX_OUTPUT_STEREO

                 };

                 VertexOutput ShadowPassVertex(VertexInput v)
                 {
                    VertexOutput o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_TRANSFER_INSTANCE_ID(v, o);
                    // UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);                             

                        float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                        float3 normalWS = TransformObjectToWorldNormal(v.normal.xyz);

                        float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));

                        o.vertex = positionCS;

                        return o;
                 }

                half4 ShadowPassFragment(VertexOutput i) : SV_TARGET
                {
                    UNITY_SETUP_INSTANCE_ID(i);
                        return 0;
                }
                
                ENDHLSL
        }
        Pass
        {
        Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            Cull Back

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            CBUFFER_END

            struct VertexInput
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

                struct VertexOutput
                {
                float4 vertex : SV_POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO

                };

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                // UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    o.vertex = TransformWorldToHClip(TransformObjectToWorld(v.vertex.xyz));

                    return o;
                }

                half4 frag(VertexOutput IN) : SV_TARGET
                {
                    return 0;
                }
                ENDHLSL
        }
        Pass
        {
            // Material options generated by graph

            Name "Outline"
            Blend One Zero, One Zero
            Cull Front
            ZTest LEqual
            ZWrite On

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Unity defined keywords

            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex vertOutline
            #pragma fragment fragOutline


            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _OutlineWidth;
            float4 _OutlineColor;
            CBUFFER_END


            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            struct VertexOutput
            {
                float4 position : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID

            };

            VertexOutput vertOutline(VertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.position = TransformObjectToHClip(v.vertex.xyz + v.normal * _OutlineWidth * 0.01);


                return o;
            }

            half4 fragOutline(VertexOutput i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                return _OutlineColor;
            }
            ENDHLSL
        } 
    }
}

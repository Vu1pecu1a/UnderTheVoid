Shader "Custom/ParticleText"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Cols("Columns Count", Int) = 10
        _Rows("Rows Count", Int) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "PreviewType" = "Plane" "Queue" = "Transparent+1" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
           // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
           // o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
           // o.Metallic = _Metallic;
           // o.Smoothness = _Glossiness;
           // o.Alpha = c.a;
            o.Emission = MainTex.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

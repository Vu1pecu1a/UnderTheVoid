Shader "CustomRenderTexture/FillAmount"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _SubTex("Texture", 2D) = "white" {}
        _FillAmount("Fill Amount", Range(0, 1)) = 1
        _Tiling("Tiling", Vector) = (1, 1, 0, 0)
        _Offset("Offset", Vector) = (0, 0, 0, 0)
    }
        SubShader
        {

            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
            }

            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off
                Cull Off
                Lighting Off
                Fog { Mode Off }

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                   // half2 texcoord : TEXCOORD0;
                };

                sampler2D _MainTex;
                sampler2D _SubTex;
                half2 _Tiling;
                half2 _Offset;
                float _FillAmount;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    //o.texcoord = v.texcoord;
                    o.color = v.color;
                    o.uv = v.uv * _Tiling.xy + _Offset.xy;;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                { 
                    fixed4 color = tex2D(_MainTex, i.uv); // hp¹Ù
                    fixed4 background = tex2D(_SubTex, i.uv); //¹è°æ
                // Adjust alpha based on fill amount
                    if (i.uv.x > _FillAmount* _Tiling.x) {
                        color.a = 0;
                    }

                return lerp(background, color, color.a);
            }
            ENDCG
        }
    }
}

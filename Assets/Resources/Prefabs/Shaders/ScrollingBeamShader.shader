Shader "Custom/ScrollingBeamShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Range(0.1, 10)) = 1
        _StartColor ("Start Color", Color) = (0, 1, 0, 1) // Green by default
        _EndColor ("End Color", Color) = (0, 1, 0, 0.5) // Fades to transparent green
        _FadeLength ("Fade Length", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 uv2 : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollSpeed;
            fixed4 _StartColor;
            fixed4 _EndColor;
            float _FadeLength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = v.uv; // Store the original UVs for gradient calculation
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Scroll the texture
                float2 uv = i.uv;
                uv.y += _Time.y * _ScrollSpeed;
                fixed4 texColor = tex2D(_MainTex, uv);

                // Calculate gradient color
                fixed4 color = lerp(_StartColor, _EndColor, i.uv2.y);

                // Apply texture and gradient color
                color *= texColor;

                // Apply fade effect
                float fade = smoothstep(0.0, _FadeLength, i.uv2.y);
                color.a *= fade;

                return color;
            }
            ENDCG
        }
    }
}

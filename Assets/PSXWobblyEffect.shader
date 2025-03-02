Shader "Custom/PSXWobblyEffect"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {} // Declare the texture property
        _WobbleSpeed ("Wobble Speed", Float) = 0.1
        _WobbleStrength ("Wobble Strength", Float) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Define the structure for appdata and v2f
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Declare the properties
            uniform float _WobbleSpeed;
            uniform float _WobbleStrength;
            uniform sampler2D _MainTex; // Declare the texture here

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;

                // Apply wobble based on time
                float3 wobble = sin(v.vertex.x * _WobbleSpeed + _Time.y) * _WobbleStrength;
                v.vertex.x += wobble.x;
                v.vertex.y += wobble.y;

                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample the texture
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

Shader "Custom/Sprite Dissolve Object"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 1
        _DissolveColor ("Dissolve Color", Color) = (1,0.2,0,1)
        _DissolveEmission ("Dissolve Emission", Float) = 2
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 objPos : TEXCOORD1;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTexture;
            fixed4 _Color;
            fixed4 _DissolveColor;
            float _DissolveAmount;
            float _DissolveEmission;
            float _NoiseScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.objPos = v.vertex.xy + 0.5;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                
                if (col.a < 0.01) discard;

                fixed noise = tex2D(_DissolveTexture, i.objPos * _NoiseScale).r;
                
                float cut = (_DissolveAmount * 1.4) - 0.2;
                
                float alphaMult = smoothstep(cut, cut + 0.15, noise);
                
                col.a *= alphaMult;
                
                float edgeGlow = (1 - alphaMult) * alphaMult * 4.0;
                col.rgb += _DissolveColor.rgb * _DissolveEmission * edgeGlow;
                
                return col;
            }
            ENDCG
        }
    }
}
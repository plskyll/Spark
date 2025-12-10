Shader "Custom/Sprite Dissolve"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
        _DissolveColor ("Dissolve Color", Color) = (1,1,1,1)
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTexture;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _DissolveColor;
            float _DissolveAmount;
            float _DissolveEmission;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                fixed dissolve = tex2D(_DissolveTexture, i.uv).r;
                
                // Створюємо ефект розчинення
                float cutoff = _DissolveAmount;
                float edge = 0.1;
                
                // Якщо пікселі нижче порогу - робимо прозорими
                if (dissolve < cutoff)
                {
                    discard;
                }
                
                // Додаємо світіння по краях розчинення
                if (dissolve < cutoff + edge)
                {
                    float edgeFactor = (dissolve - cutoff) / edge;
                    col.rgb = lerp(_DissolveColor.rgb * _DissolveEmission, col.rgb, edgeFactor);
                }
                
                return col;
            }
            ENDCG
        }
    }
}
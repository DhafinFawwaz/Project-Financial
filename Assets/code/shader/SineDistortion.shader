Shader "Custom/SpriteDistortWithShadows"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Frequency ("_Frequency", Float) = 10
        _Amplitude ("_Amplitude", Float) = 0.01
        _Speed ("_Speed", Float) = 1
        _Offset ("_Offset", Vector) = (0,0,0,0)
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
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
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade addshadow
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        fixed4 _Color;
        float _Frequency;
        float _Amplitude;
        float _Speed;
        float4 _Offset;
        sampler2D _MainTex;
        sampler2D _AlphaTex;
        float _AlphaSplitEnabled;
        float _Cutoff;

        fixed4 SampleSpriteTexture(float2 uv)
        {
            uv.x = (uv.x + _Offset.x) + (uv.y + _Offset.y) * sin((uv.y + _Offset.y) * _Frequency + _Time.y * _Speed) * _Amplitude;

            fixed4 color = tex2D(_MainTex, uv);

            #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
            if (_AlphaSplitEnabled)
                color.a = tex2D(_AlphaTex, uv).r;
            #endif // UNITY_TEXTURE_ALPHASPLIT_ALLOWED

            return color;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG

        // Shadow caster pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ColorMask 0
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv_MainTex)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f
            {
                V2F_SHADOW_CASTER;
                float2 uv_MainTex;
            };

            v2f vert(appdata_tan v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER(o);
                o.uv_MainTex = v.texcoord;
                return o;
            }

            sampler2D _MainTex;
            float _Cutoff;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv_MainTex);
                clip(col.a - _Cutoff); // Use _Cutoff to discard fragments based on alpha
                return col;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}

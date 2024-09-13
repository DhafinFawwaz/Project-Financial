Shader "Custom/SpriteCircularWipe"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Progress ("Progress", Range(0, 1)) = 0
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
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
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 tangent  : TANGENT;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 tangent   : TEXCOORD1;
                float4 worldPos  : TEXCOORD2;
            };

            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;
            float2 _Center;
            float _Progress;
            float _Smoothness;

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord.xy;
                float2 center = _Center;
                float progress = _Progress;

                float2 dir = uv - center;
                float dist = length(dir);
                float angle = atan2(dir.y, dir.x);
                float angleProgress = (angle + 3.14159265359) / 6.28318530718;

                float alpha = 1 - smoothstep(progress - _Smoothness, progress + _Smoothness, angleProgress);

                return tex2D(_MainTex, IN.texcoord) * IN.color * alpha;
            }




        ENDCG
        }
    }
}
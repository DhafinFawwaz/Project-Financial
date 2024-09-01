Shader "Custom/FakePointLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Pow ("Pow", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha One

        Cull Front Lighting Off ZWrite Off 

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            fixed _Pow;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed easeInOutPow(fixed t, fixed p)
            {
                return t < 0.5 ? pow(2.0*t, p)/2.0 : 1.0-pow(2.0*(1.0-t), p)/2.0;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 center = fixed2(0.5, 0.5);
                fixed2 uv = i.uv - center;
                fixed dist = length(uv);
                fixed alpha = 1-dist*2;
                alpha = saturate(alpha);
                
                alpha = easeInOutPow(alpha, _Pow);

                fixed4 col = fixed4(_Color.rgb, alpha*_Color.a);
                return col;
            }
            


            ENDCG
        }
    }
}
    
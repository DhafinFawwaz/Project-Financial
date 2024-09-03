Shader "Custom/2Circle"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Radius ("Radius", Range(0, 0.5)) = 0.5
        _Transparency ("Transparency", Range(0, 1)) = 1
        _Period ("Period", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

        Cull Back ZWrite Off
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            float _Radius;
            float _Transparency;
            float _Period;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float d1 = distance(i.uv, center);

                if(d1 > 0.5) discard;
                float alpha = _Transparency * 0.5 * (1.0 + sin(_Time.y / _Period));
                if(d1 < _Radius) {
                    return fixed4(_Color1.rgb, _Color1.a*alpha);
                }
                return fixed4(_Color2.rgb, _Color2.a*alpha);
            }
            ENDCG
        }
    }
}

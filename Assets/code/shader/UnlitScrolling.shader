Shader "Unlit/Scrolling" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _ScrollXSpeed ("X Scroll Speed", Range (0,10)) = 2
        _ScrollYSpeed ("Y Scroll Speed", Range (0,10)) = 0
        _Cutoff ("Alpha Cutoff", Range (0,1)) = 0.5
    }
    SubShader {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
        LOD 200
        Lighting Off

        CGPROGRAM
        #pragma surface surf Unlit alpha

        sampler2D _MainTex;
        fixed _ScrollXSpeed;
        fixed _ScrollYSpeed;
        half _Cutoff;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed2 scrolledUV = IN.uv_MainTex;
            fixed xscrollValue = _ScrollXSpeed * _Time.y;
            fixed yscrollValue = _ScrollYSpeed * _Time.y;

            scrolledUV += fixed2( xscrollValue, yscrollValue);

            half4 c = tex2D(_MainTex, scrolledUV);
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            // Apply alpha cutoff
            clip(o.Alpha - _Cutoff);
        }

        fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        ENDCG
    } 
    FallBack "Diffuse"
}

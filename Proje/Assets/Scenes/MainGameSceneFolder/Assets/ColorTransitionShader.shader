Shader "Custom/UIColorTransition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Ana resim
        _FillAmount ("Fill Amount", Range(0,1)) = 0.0 // Renk geçiþ miktarý
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex; // Resim
            float _FillAmount;  // Geçiþ deðeri

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float y = i.uv.y; // UV'nin Y ekseni
                if (y < _FillAmount)
                    return tex2D(_MainTex, i.uv); // Orijinal resim
                else
                    return fixed4(0, 0, 0, 0); // Þeffaflýk (Siyah yerine görünmez)
            }
            ENDCG
        }
    }
}

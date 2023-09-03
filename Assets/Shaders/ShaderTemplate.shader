Shader "Unlit/ShaderTemplate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            //Automatically supplied by Unity
            struct MeshData //Typically refered to as AppData - per vertex mesh data
            {
                float4 vertex : POSITION; //Vertex Position
                float2 uv : TEXCOORD0;    //UV Coordinates
                float3 normals : NORMAL;
            };

            struct v2f //Interpolators - Passes info from vertex to fragment shader
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION; //clip space position
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (MeshData v) //Vertex Shader
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //converts local space to clip space
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target //Fragment Shader
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

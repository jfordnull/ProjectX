Shader "Unlit/PikminRings"
{
    Properties
    {
        _ColorA("Color", Color) = (1,1,1,1)
        _ColorB("Color", Color) = (0,0,0,1)
        _ColorStart("Color Start", range(0,1)) = 0
        _ColorEnd("Color End", range(0,1)) = 1
    }

    SubShader
    {
        Tags 
        {
         "RenderType"="Transparent" // tag to inform the render pipeline of what type this is
         "Queue" = "Transparent"    // changes the render order
        
        }

        Pass
        {
            //pass tags
            //Blend One One //additive blending
            ZWrite Off    //prevents writing to the depth buffer
            //ZTest Always
            Cull Off //disables backface culling

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.283185307179586

            //Automatically supplied by Unity
            struct MeshData //Typically refered to as AppData - per vertex mesh data
            {
                float4 vertex : POSITION; //Vertex Position
                float2 uv : TEXCOORD0;    //UV Coordinates
                float3 normals : NORMAL;
            };

            struct v2f //Interpolators - Passes info from vertex to fragment shader
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };


            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

            v2f vert ( MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //converts local space to clip space
                o.normal = UnityObjectToWorldNormal( v.normals );
                o.uv = v.uv;
                return o;
            }

            float InverseLerp ( float a, float b, float v)
            {
                return (v-a)/(b-a);
            }

            float4 frag (v2f i) : SV_Target
            {
                float t = InverseLerp(_ColorStart, _ColorEnd, i.uv.y);
                return t;
            }
            ENDCG
        }
    }
}

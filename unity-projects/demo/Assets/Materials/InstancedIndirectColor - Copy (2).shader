// https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html
// https://github.com/Toqozz/blog-code/blob/master/mesh_batching/Assets/InstancedIndirectColor.shader
Shader "Custom/InstancedIndirectColor" 
{
    SubShader 
	{
        Tags { "RenderType" = "Opaque" }

        Pass 
	{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc" // for _LightColor0

            struct v2f {
                float4 vertex   : SV_POSITION;
		fixed4 diff : COLOR0; // diffuse lighting color
                fixed4 color    : COLOR1;
            }; 

            struct MeshProperties {
                float4x4 mat;
                float4 color;
            };

            StructuredBuffer<MeshProperties> _Properties;

            v2f vert(appdata_base i, uint instanceID: SV_InstanceID)
 	    {
                v2f o;

                float4 pos = mul(_Properties[instanceID].mat, i.vertex);
                o.vertex = UnityObjectToClipPos(pos);
                o.color = _Properties[instanceID].color;
		
		// get vertex normal in world space
                half3 worldNormal = UnityObjectToWorldNormal(mul(_Properties[instanceID].mat, i.normal));

                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                
		// factor in the light color
                o.diff = nl * _LightColor0;

                // in addition to the diffuse lighting from the main light,
                // add illumination from ambient or light probes
                // ShadeSH9 function from UnityCG.cginc evaluates it,
                // using world space normal
                o.diff.rgb += ShadeSH9(half4(worldNormal,1));

                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                return i.color * i.diff;
            }
            
            ENDCG
        }
    }
}
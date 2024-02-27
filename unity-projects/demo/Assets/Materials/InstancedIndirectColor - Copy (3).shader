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

		// compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"

            struct v2f {
                float4 vertex   : SV_POSITION;
		fixed4 diff : COLOR0; // diffuse lighting color
                fixed4 color : COLOR1;
		fixed3 ambient: COLOR2;
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

		// Not used when using shadows
                //o.diff.rgb += ShadeSH9(half4(worldNormal,1));

		o.ambient = ShadeSH9(half4(worldNormal,1));
                // compute shadows data
                TRANSFER_SHADOW(o)

                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
		fixed4 col = i.color;
		// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
                return col;
            }
            
            ENDCG
        }

	 // shadow casting support
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
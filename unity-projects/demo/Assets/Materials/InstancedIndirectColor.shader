Shader "Instanced/InstancedIndirect" 
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		// Physically based Standard lighting model
		#pragma surface surf Standard addshadow
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:setup

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		struct MeshProperties {
            float4x4 mat;
            float4 color;
        };

    StructuredBuffer<MeshProperties> _Properties;
#endif

	void setup()
	{
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		unity_ObjectToWorld = _Properties[unity_InstanceID].mat;
		unity_WorldToObject = unity_ObjectToWorld;
		unity_WorldToObject._14_24_34 *= -1;
		unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;
#endif
	}

	half _Glossiness;
	half _Metallic;

	void surf(Input IN, inout SurfaceOutputStandard o) 
	{
		float4 col = 1.0f;

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		col = _Properties[unity_InstanceID].color;
#else
		col = float4(0, 0, 1, 1);
#endif

		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * col;
		o.Albedo = c.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}
	ENDCG
	}
	FallBack "Diffuse"
}
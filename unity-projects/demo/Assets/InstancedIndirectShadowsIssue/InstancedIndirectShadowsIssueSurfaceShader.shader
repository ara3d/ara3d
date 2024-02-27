Shader "Instanced/InstancedIndirectShadowsIssue" 
{
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
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
			StructuredBuffer<float4> colorBuffer;
		#endif
		float _Dim;
		float4 _Pos;

		float rand(in float2 uv)
		{
			float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
			return abs(noise.x + noise.y) * 0.5;
		}

		void rotate2D(inout float2 v, float r)
		{
			float s, c;
			sincos(r, s, c);
			v = float2(v.x * c - v.y * s, v.x * s + v.y * c);
		}

	void setup()
	{
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		// this uv assumes the # of instances is _Dim * _Dim. 
		// so we calculate the uv inside a grid of _Dim x _Dim elements.
		float2 uv = float2( floor(unity_InstanceID / _Dim) / _Dim, (unity_InstanceID % (int)_Dim) / _Dim);
		// in this case, _Dim can be replaced by the size in the world
		float4 position = _Pos + float4(uv.x * _Dim * 2, 0, uv.y * _Dim * 2, 1);
		float scale = position.w;

		//float rotation = scale * scale * _Time.y * 0.5f;
		//rotate2D(position.xz, rotation);

		unity_ObjectToWorld._11_21_31_41 = float4(scale, 0, 0, 0);
		unity_ObjectToWorld._12_22_32_42 = float4(0, scale, 0, 0);
		unity_ObjectToWorld._13_23_33_43 = float4(0, 0, scale, 0);
		unity_ObjectToWorld._14_24_34_44 = float4(position.xyz, 1);
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
		col = colorBuffer[unity_InstanceID];
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

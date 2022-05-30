Shader "Custom/2 Textures Albedo+Normal Mix"
{
	Properties
	{
		_MainTex("Albedo (RGB), Specular (A)", 2D) = "white" {}
		_NormalTex1("Normal Map", 2D) = "bump" {}
		_NormalMapScale1("Normal Map Scale", float) = 1
		_SecondTex("Albedo (RGB), Specular (A)", 2D) = "white" {}
		_NormalTex2("Normal Map", 2D) = "bump" {}
		_NormalMapScale2("Normal Map Scale", float) = 1
		_MaskTex("Mask (R)", 2D) = "black" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		#include "UnityStandardUtils.cginc"

		sampler2D _MainTex;
		sampler2D _NormalTex1;
		sampler2D _SecondTex;
		sampler2D _NormalTex2;
		sampler2D _MaskTex;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_SecondTex;
			float2 uv_MaskTex;
		};

		float _NormalMapScale1;
		float _NormalMapScale2;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed mix = tex2D(_MaskTex, IN.uv_MaskTex).r;
			fixed4 tex1 = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 tex2 = tex2D(_SecondTex, IN.uv_SecondTex);

			o.Albedo = (1.f - mix) * tex1.rgb + mix * tex2.rgb;
			o.Smoothness = (1.f - mix) * tex1.a + mix * tex2.a;
			o.Normal = (1.f - mix) * UnpackScaleNormal(tex2D(_NormalTex1, IN.uv_MainTex), _NormalMapScale1)
				+ mix * UnpackScaleNormal(tex2D(_NormalTex2, IN.uv_SecondTex), _NormalMapScale2);
		}
		ENDCG
	}
		FallBack "Diffuse"
}

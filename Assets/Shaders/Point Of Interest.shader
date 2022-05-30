Shader "Custom/Point Of Interest"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_Value("Value", Range(0,1)) = 0
	}
		SubShader
	{
	Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
	Cull Off
	ZWrite Off
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
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			fixed4 color : COLOR;
			float2 texcoord  : TEXCOORD0;
		};

		fixed4 _Color;
		fixed4 _Color2;
		fixed _Value;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.color = v.color;
			o.texcoord = v.texcoord;
			return o;
		}

		float square(const float x)
		{
			return x * x;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			half4 color = lerp(_Color, _Color2, step(i.texcoord.x, _Value));
			color.a = square(i.color.r)*color.a;
			return color;
		}
	ENDCG
	}
	}
}

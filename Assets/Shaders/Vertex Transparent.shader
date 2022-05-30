Shader "Custom/Vertex Transparent Square"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
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

		struct appdata
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			fixed4 color : COLOR;
		};

		fixed4 _Color;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.color = v.color;
			return o;
		}

		float square(const float x)
		{
			return x * x;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			half4 color = _Color;
			color.a = square(i.color.r)*color.a;
			return color;
		}
	ENDCG
	}
	}
}

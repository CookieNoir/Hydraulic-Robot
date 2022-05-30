Shader "Custom/Vertex Transparent Gradient"
{
	Properties
	{
		_Color("Color Low (RG)", Color) = (1,1,1,1)
		_Color2("Color High (RB)", Color) = (1,1,1,1)

		_leftOffset("Left Offset (R)", Range(0,1)) = 0
		_rightOffset("Right Offset (R)", Range(0,1)) = 1
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
		fixed4 _Color2;
		fixed _leftOffset;
		fixed _rightOffset;

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
			float r = i.color.r, g = i.color.g, b = i.color.b;
			float s1 = step(_leftOffset, r), s2 = step(_rightOffset, r), sg = step(r, g), sb = step(g, b);
			half4 color = (1-sg)*((1-s1)*_Color+s1*((1-s2)*lerp(_Color,_Color2,(r-_leftOffset)/(_rightOffset-_leftOffset))+s2*_Color2)) + sg*((1-sb)*_Color + sb*_Color2);
			color.a = (r+square(g)+square(b))*color.a;
			return color;
		}
	ENDCG
	}
	}
}

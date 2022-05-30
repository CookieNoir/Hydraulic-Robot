Shader "Custom/Line For Quest"
{
	Properties
	{
		_MainTex("Direction (R)", 2D) = "white" {}
		_Color1("Color", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_Color3("Color 3", Color) = (1,1,1,1)
		_Value1("Value Lower", Range(0,1)) = 0
		_Value2("Value Upper", Range(0,1.001)) = 0
		_ScaleX("Scale X", float) = 1.0
	}
		SubShader
		{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
		ZWrite On
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
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
				float2 texcoord2 : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color1;
			fixed4 _Color2;
			fixed4 _Color3;
			fixed _Value1;
			fixed _Value2;
			float _ScaleX;
			static const fixed4 clear = fixed4(0.0, 0.0, 0.0, 0.0);

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.texcoord2 = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord2.x = o.texcoord2.x * _ScaleX - 2.0 * _Time.y;
				return o;
			}

			float cube(float x)
			{
				return x * x * x;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = clear;
				fixed red = tex2D(_MainTex, i.texcoord2).r;
				float f = max((i.texcoord.x - _Value1) / (_Value2 - _Value1), 0.0);
				float grad = 1.0 - cube(max(0.0, abs(2.0*f - 1.0) * 2.0 - 1.0));
				fixed4 midColor = (1.0 - red) * _Color1 + red * _Color3;
				color += lerp(clear, _Color2, step(i.texcoord.x, _Value1));
				color += lerp(lerp(lerp(_Color2, midColor, grad), lerp(_Color1, midColor, grad), step(0.5, f)), clear, step((_Value2 - i.texcoord.x)*sign(i.texcoord.x - _Value1), 0.0));
				color += lerp(clear, _Color1, step(_Value2, i.texcoord.x));
				return color;
			}
		ENDCG
		}
		}
}

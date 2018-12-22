Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Thickness ("Thickness", Range(0,10)) = 0.5

		_MainTex ("transparent", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }

		LOD 100
         Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _Thickness;
			
			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				// Slightly enlarge our quad, so we have a margin around it to draw the outline.
				float expand = 1.1f;
				v.vertex.xyz *= expand;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// If we want to get fancy, we could compute the expansion 
				// dynamically based on line thickness & view angle, but I'm lazy)

				// Expand the texture coordinate space by the same margin, symmetrically.
				o.uv = (v.uv - 0.5f) * expand + 0.5f;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// Texcoord distance from the center of the quad.
				float2 fromCenter = abs(i.uv - 0.5f);
				// Signed distance from the horizontal & vertical edges.
				float2 fromEdge = fromCenter - 0.5f;

				// Use screenspace derivatives to convert to pixel distances.
				fromEdge.x /= length(float2(ddx(i.uv.x), ddy(i.uv.x)));
				fromEdge.y /= length(float2(ddx(i.uv.y), ddy(i.uv.y)));

				// Compute a nicely rounded distance from the edge.
				float distance = abs(min(max(fromEdge.x,fromEdge.y), 0.0f) + length(max(fromEdge, 0.0f)));

				// Sample our texture for the interior.
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;

				// get rid of the middle part of the texture
				col.a = 0.0f;
				// Clip out the part of the texture outside our original 0...1 UV space.
				//col.a *= step(max(fromCenter.x, fromCenter.y), 0.5f);

				// Blend in our outline within a controllable thickness of the edge.
				col = lerp(col, _Color, saturate(_Thickness - distance));
				//col = lerp(col, _Color, saturate(_Thickness - distance));

				return col;
			}
			ENDCG
		}
	}
}

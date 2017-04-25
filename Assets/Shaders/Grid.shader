// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
Shader "Custom/Grid" {

	Properties {
		_GridThickness ("Grid Thickness", Float) = 0.01
		_GridSpacing ("Grid Spacing", Float) = 10.0
		_Tiling ("Tiling", Float) = 1.0
		_GridColour ("Grid Colour", Color) = (1.0, 1.0, 1.0, 1.0)
		_BaseColour ("Base Colour", Color) = (1.0, 0.0, 0.0, 0.0)
	}

	SubShader {
		Tags { "Queue" = "Transparent" }

		Pass {
			ZTest Always
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			// Define the vertex and fragment shader functions
			#pragma vertex vert
			#pragma fragment frag

			// Access Shaderlab properties
			uniform float _GridThickness;
			uniform float _GridSpacing;
			uniform float _Tiling;
			uniform float4 _GridColour;
			uniform float4 _BaseColour;

			// Input into the vertex shader
			struct vertexInput {
				float4 vertex : POSITION;
			};

			// Output from vertex shader into fragment shader
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};

			// VERTEX SHADER
			vertexOutput vert(vertexInput input) {
				vertexOutput output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				// Calculate the world position coordinates to pass to the fragment shader
//				output.worldPos = mul(unity_ObjectToWorld, input.vertex);
				output.worldPos = input.vertex;
				return output;
			}

			// FRAGMENT SHADER
			float4 frag(vertexOutput input) : COLOR {
				float tiledSpacing = _GridSpacing / _Tiling;
				float tiledThickness = _GridThickness / _Tiling;
				
				float xfrac = frac(input.worldPos.x / tiledSpacing) - 0.5;
//				float zfrac = frac(input.worldPos.z / tiledSpacing) - 0.5;
				float yfrac = frac(input.worldPos.y / tiledSpacing) - 0.5;
//				float dist = xfrac * xfrac + zfrac * zfrac;
				float dist = xfrac * xfrac + yfrac * yfrac;
				float strength = max(0.5 - dist, 0) * 2;
				strength = pow(strength, 5);

				bool xvalid = false;
				bool yvalid = false;


				if (input.worldPos.x > 0)
				{
					if (fmod(input.worldPos.x, tiledSpacing) < tiledThickness)
					xvalid = true;
				}
				else
				{
					if (fmod(input.worldPos.x, tiledSpacing) < tiledThickness - tiledSpacing)
					xvalid = true;
				}

				if (input.worldPos.y > 0)
				{
					if (fmod(input.worldPos.y, tiledSpacing) < tiledThickness)
					yvalid = true;
				}
				else
				{
					if (fmod(input.worldPos.y, tiledSpacing) < tiledThickness - tiledSpacing)
					yvalid = true;
				}

				if (xvalid && yvalid)
					return _GridColour * strength + _BaseColour * (1.0 - strength);
				else
					return _BaseColour;
			}
			ENDCG
		}
	}
}
﻿// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/BlinkShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_BlinkColor("BlinkColor", Color) = (1,1,1,1)
		_BlinkLimit("BlinkLimit", Range(0.0, 0.99)) = 0.5
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex SpriteVert
				#pragma fragment BlinkSpriteFrag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"

				fixed4 _BlinkColor;
				fixed  _BlinkLimit;

				//Change the sprite to solid white when alhpa is less than _BlinkLimit
				fixed4 BlinkSpriteFrag(v2f IN) : SV_Target
				{
					fixed4 texel = SampleSpriteTexture(IN.texcoord);
					float showTexel = step(_BlinkLimit, IN.color.a);

					IN.color.a = (IN.color.a - _BlinkLimit) / (1.0 - _BlinkLimit);

					//Only turns white when alpha is greater that 0.1,
					//otherwise ignore
					float aAfter = 0;

					if (texel.a > 0.2) {
						aAfter = 1;
					}
					else {
						aAfter = 0;
					}

					fixed4 c = texel * IN.color * showTexel + _BlinkColor * (1.0 - showTexel) * sign(aAfter);

					c.rgb *= c.a;
					return c;
				}

			ENDCG
			}
		}
}
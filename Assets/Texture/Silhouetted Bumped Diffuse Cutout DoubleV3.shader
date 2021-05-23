﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Outlined/Silhouetted Bumped Diffuse Cutout DoubleV3" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	_Outline ("Outline width", Range (0.0, 0.03)) = .005
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
    float4 texcoord : TEXCOORD;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
    float4 uv : TEXCOORD;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;

sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
fixed _Cutoff;

v2f vert(appdata v) {
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord;
 
	float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * _Outline;
	o.color = _OutlineColor;
	return o;
}
ENDCG


	SubShader {
		Tags { "Queue" = "Transparent" }

 		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
//			Cull Off	//I don't want to cull the outline @_@
			ZWrite Off
//			ZTest Always	//??
 			Blend SrcAlpha OneMinusSrcAlpha // Normal

CGPROGRAM
#pragma vertex vert
#pragma fragment frag

 
half4 frag(v2f i) : COLOR {

    fixed4 texCol = tex2D(_MainTex, i.uv);
    if(texCol.a < _Cutoff)
        discard;

	return i.color;
}
ENDCG
}
 
 Cull Off	//It works here for some reason...



CGPROGRAM
#pragma surface surf Lambert
struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf(Input IN, inout SurfaceOutput o) {

    fixed4 texCol = tex2D(_MainTex, IN.uv_MainTex);

    if(texCol.a < _Cutoff)
        discard;

	o.Albedo = texCol.rgb * _Color;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Alpha = c.a;
}
ENDCG
}


	SubShader {
		Tags { "Queue" = "Transparent" }
 
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
//			Cull Off	//Doesn't seem to do anything?
			ZWrite Off
//			ZTest Always	//??
			Offset 15,15
 			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
#pragma vertex vert
#pragma fragment frag

 
half4 frag(v2f i) : COLOR {

    fixed4 texCol = tex2D(_MainTex, i.uv);
    if(texCol.a < _Cutoff)
        discard;

    return i.color;
}
ENDCG

			}


CGPROGRAM
#pragma surface surf Lambert
struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf(Input IN, inout SurfaceOutput o) {

    fixed4 texCol = tex2D(_MainTex, IN.uv_MainTex);

    if(texCol.a < _Cutoff)
        discard;

    o.Albedo = texCol.rgb * _Color;

	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Alpha = c.a;
}
ENDCG
}
	Fallback "Standard"
}
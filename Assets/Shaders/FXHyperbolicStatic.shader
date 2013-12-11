// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

//
// Shader: "FX/Hyperbolic Static"
// Version: v1.0
// Written by: Thomas Phillips
//
// Anyone is free to use this shader for non-commercial or commercial projects.
//
// Description:
// Generic force field effect.
// Play with color, opacity, and rate for different effects.
//

Shader "FX/Hyperbolic Static" {
    
Properties {
    _Color ("Color Tint", Color) = (1,1,1,1)
    _Rate ("Oscillation Rate", Range (1, 300)) = 300
}

SubShader {
    
    ZWrite Off
    Tags { "Queue" = "Transparent" }
    Blend One One

    Pass {

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_fog_exp2
#include "UnityCG.cginc"

float4 _Color;
float _Rate;

struct v2f {
    float4 pos : SV_POSITION;
    float4 texcoord : TEXCOORD0;
};

v2f vert (appdata_base v)
{
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.texcoord = v.texcoord;
    return o;
}

half4 frag (v2f i) : COLOR
{
    float3 color;
    float m;
    m = _Time[0]*_Rate + ((i.texcoord[0]+i.texcoord[1])*5000000*_Color.a*_Color.a);
    m = sin(m) * 0.5;
    color = float3(m*_Color.r, m*_Color.g, m*_Color.b);
    return half4( color, 1 );
}
ENDCG

    }
}
Fallback "Transparent/Diffuse"
}
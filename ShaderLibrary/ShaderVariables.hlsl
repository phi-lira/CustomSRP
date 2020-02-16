#ifndef SHADER_VARIABLES_HLSL
#define SHADER_VARIABLES_HLSL
// Shader variable as defined in: https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html

///////////////////////////////////////////////////////////////////////////////
// Transformations                                                            /
///////////////////////////////////////////////////////////////////////////////
float4x4 unity_MatrixV;
float4x4 unity_MatrixP;
float4x4 unity_MatrixVP;

float4x4 unity_MatrixInvV;
float4x4 unity_MatrixInvP;
float4x4 unity_MatrixInvVP;

#define UNITY_MATRIX_M     unity_ObjectToWorld
#define UNITY_MATRIX_I_M   unity_WorldToObject
#define UNITY_MATRIX_V     unity_MatrixV
#define UNITY_MATRIX_P     OptimizeProjectionMatrix(unity_MatrixP)
#define UNITY_MATRIX_VP    unity_MatrixVP

#define UNITY_MATRIX_I_V   unity_MatrixInvV
#define UNITY_MATRIX_I_P   unity_MatrixInvP
#define UNITY_MATRIX_I_VP  unity_MatrixInvVP

#define UNITY_MATRIX_MV    mul(UNITY_MATRIX_V, UNITY_MATRIX_M)
#define UNITY_MATRIX_T_MV  transpose(UNITY_MATRIX_MV)
#define UNITY_MATRIX_IT_MV transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))
#define UNITY_MATRIX_MVP   mul(UNITY_MATRIX_VP, UNITY_MATRIX_M)

///////////////////////////////////////////////////////////////////////////////
// Camera and Screen                                                          /
///////////////////////////////////////////////////////////////////////////////
float3 _WorldSpaceCameraPos;

// x = 1 or -1 (-1 if projection is flipped)
// y = near plane
// z = far plane
// w = 1/far plane
float4 _ProjectionParams;

// x = width
// y = height
// z = 1 + 1.0/width
// w = 1 + 1.0/height
float4 _ScreenParams;

// Values used to linearize the Z buffer (http://www.humus.name/temp/Linearize%20depth.txt)
// x = 1-far/near
// y = far/near
// z = x/far
// w = y/far
// or in case of a reversed depth buffer (UNITY_REVERSED_Z is 1)
// x = -1+far/near
// y = 1
// z = x/far
// w = 1/far
float4 _ZBufferParams;

// x = orthographic camera's width
// y = orthographic camera's height
// z = unused
// w = 1.0 if camera is ortho, 0.0 if perspective
float4 unity_OrthoParams;

float4x4 unity_CameraProjection;
float4x4 unity_CameraInvProjection;
float4 unity_CameraWorldClipPlanes[6];

float4x4 OptimizeProjectionMatrix(float4x4 M)
{
    // Matrix format (x = non-constant value).
    // Orthographic Perspective  Combined(OR)
    // | x 0 0 x |  | x 0 x 0 |  | x 0 x x |
    // | 0 x 0 x |  | 0 x x 0 |  | 0 x x x |
    // | x x x x |  | x x x x |  | x x x x | <- oblique projection row
    // | 0 0 0 1 |  | 0 0 x 0 |  | 0 0 x x |
    // Notice that some values are always 0.
    // We can avoid loading and doing math with constants.
    M._21_41 = 0;
    M._12_42 = 0;
    return M;
}

///////////////////////////////////////////////////////////////////////////////
// Time                                                                       /
///////////////////////////////////////////////////////////////////////////////
// Time (t = time since current level load) values from Unity
float4 _Time; // (t/20, t, t*2, t*3)
float4 _SinTime; // sin(t/8), sin(t/4), sin(t/2), sin(t)
float4 _CosTime; // cos(t/8), cos(t/4), cos(t/2), cos(t)
float4 unity_DeltaTime; // dt, 1/dt, smoothdt, 1/smoothdt

///////////////////////////////////////////////////////////////////////////////
// Fog and Ambient                                                            /
///////////////////////////////////////////////////////////////////////////////
half4 unity_AmbientSky;
half4 unity_AmbientEquator;
half4 unity_AmbientGround;
half4 unity_FogColor;
half4 unity_FogParams;

///////////////////////////////////////////////////////////////////////////////
// Per-object variable                                                        /
///////////////////////////////////////////////////////////////////////////////
// Block Layout should be respected due to SRP Batcher
CBUFFER_START(UnityPerDraw)
// Space block Feature
float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObject;
float4 unity_LODFade; // x is the fade value ranging within [0,1]. y is x quantized into 16 levels
half4 unity_WorldTransformParams; // w is usually 1.0, or -1.0 for odd-negative scale transforms

half4 unity_ProbesOcclusion;

// Lightmap block feature
float4 unity_LightmapST;
float4 unity_DynamicLightmapST;

// SH block feature
real4 unity_SHAr;
real4 unity_SHAg;
real4 unity_SHAb;
real4 unity_SHBr;
real4 unity_SHBg;
real4 unity_SHBb;
real4 unity_SHC;
CBUFFER_END

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
#endif
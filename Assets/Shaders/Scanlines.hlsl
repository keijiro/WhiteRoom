#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"

void Scroller_float(float3 WPos, out float Mask, out float Line)
{
    float y1 = WPos.y * 0.5 - _Time.y;
    float y2 = WPos.y * 90;
    Mask = frac(y1) > 0.8;
    Line = frac(y2) > 0.8;

    Mask = saturate(SimplexNoise(float3(0.2, _Time.y * 8, 0.5)));
    Mask = smoothstep(0, 0.9, Mask);

    Line *= (1 - Mask);
}

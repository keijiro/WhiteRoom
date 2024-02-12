#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"

void BodyShader_float
  (float3 WorldPosition,
   float3 LineParams,
   float3 LineColor,
   float3 TextureColor,
   float TextureIntensity,
   float3 NoiseParams,
   float AudioInput,
   float Threshold,
   out float3 OutAlbedo,
   out float3 OutEmission,
   out float OutAlpha)
{
    float y = frac(WorldPosition.y * LineParams.x + _Time.y * LineParams.y);
    float l = 1 - smoothstep(0, LineParams.z, abs(y - 0.5) * 2);

    float n = SimplexNoise(WorldPosition * NoiseParams.x);
    l = saturate(l + min(0, n));
    l += l * pow(max(0, n), NoiseParams.z) * NoiseParams.y;

    float a = lerp(-0.2, 2, Threshold) - WorldPosition.y;
    a += 0.02 * SimplexNoise(WorldPosition * 15);
    l += (1 - smoothstep(0.0, 0.02, a)) * 2000;

    OutAlbedo = AudioInput * TextureColor * TextureIntensity;
    OutEmission = smoothstep(0.5, 1, 1 - AudioInput) * LineColor * l;
    OutAlpha = a + 0.5;
}

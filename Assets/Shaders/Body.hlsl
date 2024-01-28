void BodyShader_float
  (float3 WorldPosition,
   float LineFrequency,
   float LineWidth,
   float3 LineColor,
   float3 TextureColor,
   float TextureIntensity,
   float AudioInput,
   out float3 OutAlbedo,
   out float3 OutEmission)
{
    float y = frac(WorldPosition.y * LineFrequency);
    float l = 1 - smoothstep(0, LineWidth, abs(y - 0.5) * 2);
    OutAlbedo = AudioInput * TextureColor * TextureIntensity;
    OutEmission = smoothstep(0.5, 1, 1 - AudioInput) * LineColor * l;
}

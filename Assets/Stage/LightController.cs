using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Rendering.HighDefinition;

namespace WhiteRoom {

public sealed class LightController : MonoBehaviour
{
    HDAdditionalLightData[] _lights;

    void Start()
      => _lights = GetComponentsInChildren<HDAdditionalLightData>();

    /*
    Color GetLightColor(float3 p, float t)
    {
        var x = noise.snoise(p.xz + math.float2(0, t * 0.4f));
        var hue = math.frac(0.8f + 0.2f * x);
        return Color.HSVToRGB(hue, 0.8f, 1);
    }
    */

    Color GetLightColor(float3 p, float t)
    {
        var x = noise.snoise(p.xz + math.float2(0, t * 0.1f));
        var hue = math.frac(0.5f + 0.2f * x);
        return Color.HSVToRGB(hue, 0.4f, 1);
    }

    void Update()
    {
        foreach (var l in _lights)
        {
            l.color = GetLightColor(l.transform.localPosition, Time.time);
            l.intensity = 600;
        }
    }
}

} // namespace WhiteRoom

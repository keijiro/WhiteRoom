using UnityEngine;
using Unity.Mathematics;

namespace WhiteRoom {

public sealed class LightController : MonoBehaviour
{
    Light[] _lights;

    void Start()
      => _lights = GetComponentsInChildren<Light>();

    Color GetLightColor(float3 p, float t)
    {
        var hue = noise.snoise(math.float3(p.xz, t)) * 0.3f + 0.5f;
        return Color.HSVToRGB(hue, 1, 1);
    }

    void Update()
    {
        foreach (var l in _lights)
            l.color = GetLightColor(l.transform.localPosition, Time.time);
    }
}

} // namespace WhiteRoom

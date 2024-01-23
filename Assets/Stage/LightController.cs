using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Rendering.HighDefinition;

namespace WhiteRoom {

[ExecuteInEditMode]
public sealed class LightController : MonoBehaviour
{
    [field:SerializeField] public uint2 Dimensions { get; set; }
    [field:SerializeField] public float Size { get; set; }
    [field:SerializeField] public float Margin { get; set; }
    [field:SerializeField] public float Intensity { get; set; }

    List<GameObject> _instances = new List<GameObject>();
    bool _shouldReset;

    void OnValidate()
    {
        Dimensions = math.min(Dimensions, 8);
        _shouldReset = true;
    }

    void Start()
      => _shouldReset = true;

    void Update()
    {
        if (!_shouldReset) return;

        foreach (var go in _instances) DestroyImmediate(go);
        _instances.Clear();

        for (var z = 0; z < Dimensions.y; z++)
        {
            for (var x = 0; x < Dimensions.x; x++)
            {
                var p = math.float2(x, z);
                p -= (float2)(Dimensions - 1) * 0.5f;
                p *= Size;

                var go = new GameObject("Light");
                go.hideFlags = HideFlags.DontSave;
                go.transform.parent = transform;
                go.transform.localPosition = math.float3(p.x, 0, p.y);

                var light = go.AddComponent<Light>();
                var hdlight = go.AddComponent<HDAdditionalLightData>();

                hdlight.intensity = Intensity;

                _instances.Add(go);
            }
        }

        _shouldReset = false;
    }

    /*
    HDAdditionalLightData[] _lights;

    void Start()
      => _lights = GetComponentsInChildren<HDAdditionalLightData>();

    Color GetLightColor(float3 p, float t)
    {
        var x = noise.snoise(p.xz + math.float2(0, t * 0.4f));
        var hue = math.frac(0.8f + 0.2f * x);
        return Color.HSVToRGB(hue, 0.8f, 1);
    }

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
    */
}

} // namespace WhiteRoom

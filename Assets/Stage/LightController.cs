using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Rendering.HighDefinition;

namespace WhiteRoom {

[ExecuteInEditMode]
public sealed class LightController : MonoBehaviour
{
    [field:SerializeField] public Vector2Int Dimensions { get; set; } = new Vector2Int(4, 4);
    [field:SerializeField] public float Size { get; set; } = 4;
    [field:SerializeField] public float Margin { get; set; } = 0.1f;

    bool _toBeReset;

    List<GameObject> _instances = new List<GameObject>();

    void OnValidate()
    {
        _toBeReset = true;
    }

    void Start()
      => _toBeReset = true;

    void Update()
    {
        if (!_toBeReset) { Debug.Log("HOGE"); return;}

        Debug.Log(Dimensions);

        foreach (var go in _instances)DestroyImmediate(go);
        _instances.Clear();

        for (var z = 0; z < Dimensions.y; z++)
        {
            for (var x = 0; x < Dimensions.x; x++)
            {
                var go = new GameObject("Light");
                go.hideFlags = HideFlags.DontSave;
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(x, 0, z) * Size;
                go.AddComponent<Light>();
                _instances.Add(go);
            }
        }

        _toBeReset = false;
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

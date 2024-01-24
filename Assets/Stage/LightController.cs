using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Rendering.HighDefinition;

namespace WhiteRoom {

[ExecuteInEditMode]
public sealed class LightController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public uint2 Dimensions { get; set; }
    [field:SerializeField] public float Size { get; set; }
    [field:SerializeField] public float Margin { get; set; }
    [field:SerializeField] public float Intensity { get; set; }
    [field:SerializeField] public uint Animation { get; set; }

    #endregion

    #region Light instance management

    List<(GameObject go, HDAdditionalLightData light)> _instances =
      new List<(GameObject, HDAdditionalLightData)>();

    bool _shouldReset;

    void RemoveInstances()
    {
        foreach (var i in _instances) DestroyImmediate(i.go);
        _instances.Clear();
    }

    void PopulateInstances()
    {
        for (var x = 0; x < Dimensions.x; x++)
            for (var z = 0; z < Dimensions.y; z++)
                _instances.Add(CreateInstance(x, z));
    }

    (GameObject, HDAdditionalLightData) CreateInstance(int column, int row)
    {
        var p = math.float2(column, row);
        p -= (float2)(Dimensions - 1) * 0.5f;
        p *= Size;

        var go = new GameObject("Light (Hidden)");
        go.hideFlags = HideFlags.HideAndDontSave;

        go.transform.parent = transform;
        go.transform.localPosition = math.float3(p.x, 0, p.y);
        go.transform.localRotation = quaternion.RotateX(math.PI / 2);

        var light = go.AddComponent<Light>();
        light.type = LightType.Rectangle;

        var hdlight = go.AddComponent<HDAdditionalLightData>();
        hdlight.displayAreaLightEmissiveMesh = true;
        hdlight.SetAreaLightSize((float2)(Size - Margin));

        return (go, hdlight);
    }

    #endregion

    #region Light color animation

    Color GetLightColor(float3 p, float t)
      => Animation switch
         {  0 => GetLightColor1(p, t),
            1 => GetLightColor2(p, t),
            2 => GetLightColor3(p, t),
            _ => GetLightColor4(p, t) };

    Color GetLightColor1(float3 p, float t)
    {
        var x = noise.snoise(p.xz + math.float2(0, t * 0.4f));
        var v = math.saturate(1 + 0.8f * x);
        return Color.HSVToRGB(1, 0, v);
    }

    Color GetLightColor2(float3 p, float t)
    {
        var x = noise.snoise(p.xz + math.float2(0, t * 0.4f));
        var hue = math.frac(0.8f + 0.2f * x);
        return Color.HSVToRGB(hue, 0.8f, 1);
    }

    Color GetLightColor3(float3 p, float t)
    {
        var x = noise.snoise(p.xz + math.float2(0, t * 0.1f));
        var hue = math.frac(0.5f + 0.2f * x);
        return Color.HSVToRGB(hue, 0.4f, 1);
    }

    Color GetLightColor4(float3 p, float t)
    {
        var hue = math.frac((p.x + p.z + 0.8f * t) * 0.3f);
        return Color.HSVToRGB(hue, 0.9f, 1);
    }

    #endregion

    #region MonoBehaviour implementation

    void OnValidate()
    {
        Dimensions = math.min(Dimensions, 8);
        Size = math.max(Size, 0);
        Margin = math.max(Margin, 0);
        Intensity = math.max(Intensity, 0);
        _shouldReset = true;
    }

    void Start()
      => _shouldReset = true;

    void Update()
    {
        if (_shouldReset)
        {
            RemoveInstances();
            PopulateInstances();
            _shouldReset = false;
        }

        foreach (var (go, light) in _instances)
        {
            light.color = GetLightColor(go.transform.localPosition, Time.time);
            light.intensity = Intensity;
        }
    }

    #endregion
}

} // namespace WhiteRoom

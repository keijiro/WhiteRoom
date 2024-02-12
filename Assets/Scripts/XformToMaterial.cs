using UnityEngine;

namespace WhiteRoom {

[ExecuteInEditMode]
public sealed class XformToMaterial : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public Transform Source { get; set; }
    [field:SerializeField] public Renderer Target { get; set; }
    [field:SerializeField] public string Property { get; set; } = "_Input";

    #endregion

    #region MonoBehaviour implmementation

    MaterialPropertyBlock _props;

    void LateUpdate()
    {
        if (Source == null || Target == null) return;

        if (_props == null) _props = new MaterialPropertyBlock();

        var input = Mathf.Clamp01(Source.localPosition.y);

        Target.GetPropertyBlock(_props);
        _props.SetFloat(Property, input);
        Target.SetPropertyBlock(_props);
    }

    #endregion
}

} // namespace WhiteRoom


using UnityEngine;
using Unity.Mathematics;

namespace WhiteRoom {

[ExecuteInEditMode]
public sealed class MidiMixer : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public Transform Kick { get; set; }
    [field:SerializeField] public Transform Snare { get; set; }
    [field:SerializeField] public Transform Hat { get; set; }
    [field:SerializeField] public Renderer Target { get; set; }
    [field:SerializeField] public LightController Light { get; set; }

    #endregion

    #region MonoBehaviour implmementation

    MaterialPropertyBlock _props;

    void LateUpdate()
    {
        if (Kick == null || Snare == null || Hat == null) return;

        if (_props == null) _props = new MaterialPropertyBlock();

        var input = Kick.localPosition.y;
        input += Snare.localPosition.y;
        input += Hat.localPosition.y;
        input = math.saturate(input);

        Target?.GetPropertyBlock(_props);
        _props.SetFloat("_AudioInput", input);
        Target?.SetPropertyBlock(_props);
    }

    #endregion
}

} // namespace WhiteRoom


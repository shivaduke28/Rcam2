using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Rcam2 {

[AddComponentMenu("VFX/Property Binders/Rcam/Metadata Binder")]
[VFXBinder("Rcam/Metadata")]
class VFXRcamMetadataBinder : VFXBinderBase
{
    public string ColorMapProperty
      { get => (string)_colorMapProperty;
        set => _colorMapProperty = value; }

    public string DepthMapProperty
      { get => (string)_depthMapProperty;
        set => _depthMapProperty = value; }

    public string ProjectionVectorProperty
      { get => (string)_projectionVectorProperty;
        set => _projectionVectorProperty = value; }

    public string InverseViewMatrixProperty
      { get => (string)_inverseViewMatrixProperty;
        set => _inverseViewMatrixProperty = value; }

    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    ExposedProperty _colorMapProperty = "ColorMap";

    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    ExposedProperty _depthMapProperty = "DepthMap";

    [VFXPropertyBinding("UnityEngine.Vector4"), SerializeField]
    ExposedProperty _projectionVectorProperty = "ProjectionVector";

    [VFXPropertyBinding("UnityEngine.Matrix4x4"), SerializeField]
    ExposedProperty _inverseViewMatrixProperty = "InverseViewMatrix";

    public Camera Camera = null;
    public RcamReceiver Receiver = null;

    public override bool IsValid(VisualEffect component)
      => Camera != null && Receiver != null &&
         component.HasTexture(_colorMapProperty) &&
         component.HasTexture(_depthMapProperty) &&
         component.HasVector4(_projectionVectorProperty) &&
         component.HasMatrix4x4(_inverseViewMatrixProperty);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetTexture(_colorMapProperty, Receiver.ColorTexture);
        component.SetTexture(_depthMapProperty, Receiver.DepthTexture);
        component.SetVector4(_projectionVectorProperty, MakeProjectionVector());
        component.SetMatrix4x4(_inverseViewMatrixProperty, Camera.cameraToWorldMatrix);
    }

    public override string ToString()
    {
        var name1 = Camera == null ? "(null)" : Camera.name;
        var name2 = Receiver == null ? "(null)" : Receiver.name;
        return $"Rcam Metadata : {name1}, {name2}";
    }

    Vector4 MakeProjectionVector()
    {
        var proj = Camera.projectionMatrix;
        return new Vector4(proj[0, 2], proj[1, 2], proj[0, 0], proj[1, 1]);
    }
}

} // namespace Rcam2
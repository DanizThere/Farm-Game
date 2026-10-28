using UnityEngine;

public class Clicker : Switcher
{
    [SerializeField] private SkinnedMeshRenderer _mesh;
    private int _meshIndex;

    private void Awake()
    {
        _meshIndex = _mesh.sharedMesh.GetBlendShapeIndex("Switch");
    }

    public override void Activate()
    {
        _mesh.SetBlendShapeWeight(_meshIndex, 100);
    }

    public override void Deactivate()
    {
        _mesh.SetBlendShapeWeight(_meshIndex, 0);
    }
}

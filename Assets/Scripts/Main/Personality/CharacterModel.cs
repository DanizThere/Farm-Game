using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _bodyMesh;
    [Range(0f, 100f)]
    [SerializeField] private float _breastsSize = 60;
    [Range(0f, 100f)]
    [SerializeField] private float _hipsSize = 60;
    [Range(0f, 100f)]
    [SerializeField] private float _waistSize = 60;

    private void Update()
    {
        var breastIndex = _bodyMesh.sharedMesh.GetBlendShapeIndex("Breast");
        var waistIndex = _bodyMesh.sharedMesh.GetBlendShapeIndex("Waist");
        var hipsIndex = _bodyMesh.sharedMesh.GetBlendShapeIndex("Hips");

        _bodyMesh.SetBlendShapeWeight(breastIndex, _breastsSize);
        _bodyMesh.SetBlendShapeWeight(waistIndex, _waistSize);
        _bodyMesh.SetBlendShapeWeight(hipsIndex, _hipsSize);
    }
}

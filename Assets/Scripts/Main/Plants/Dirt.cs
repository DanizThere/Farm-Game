using System;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    public Plant Plant => _currentPlant;
    public Vector2Int DirtPosition => _dirtPosition;
    [SerializeField] private SkinnedMeshRenderer _mesh;
    [SerializeField] private PlantType _plantType;
    [SerializeField] private Transform _plantTransform;
    [SerializeField] private ParticleSystem _digParticles;
    private Vector2Int _dirtPosition;
    private Plant _currentPlant;
    private bool _canPlace = true;
    private int _meshIndex;

    public void InitializePosition(int x, int y)
    {
        _dirtPosition = new Vector2Int(x, y);
        transform.localPosition = new Vector3(x, 0, y);

        _meshIndex = _mesh.sharedMesh.GetBlendShapeIndex("Digged");
    }

    public void PlacePlant(Plant plant, System.Action onSuccess = null)
    {
        if (!_canPlace) return;
        if (_currentPlant != null) return;
        if (plant == null) return;

        var go = Instantiate(plant);
        _currentPlant = go;
        _currentPlant.transform.SetParent(_plantTransform);
        _currentPlant.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _currentPlant.OnDig += ClearPlant;
        _currentPlant.OnDeath += DestroyPlant;
        _currentPlant.StartLife(_plantType);

        _canPlace = false;
        _mesh.SetBlendShapeWeight(_meshIndex, 0);

        onSuccess?.Invoke();
    }

    private void ClearPlant()
    {
        _currentPlant = null;
    }

    private void DestroyPlant()
    {
        Destroy(_currentPlant.gameObject);
        _currentPlant = null;
    }

    public void Dig()
    {
        //here another plants should react

        _digParticles.Play();

        if (_currentPlant != null)
        {
            _currentPlant.OnDig?.Invoke();
        }

        _canPlace = true;
        _mesh.SetBlendShapeWeight(_meshIndex, _canPlace ? 100 : 0);
    }

    public void TryEffort(ToolItemObject item)
    {
        if(_currentPlant != null)
        {
            _currentPlant.TryEffort(item);
        }
    }
}

public enum PlantType
{
    Chernozems,
    Podzolic,
    Tundra
}

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
    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _digSound;
    [SerializeField] private AudioClip _plantSound;
    private Vector2Int _dirtPosition;
    private Plant _currentPlant;
    private bool _canPlace = false;
    private int _meshIndex;

    public void InitializePosition(int x, int y)
    {
        _dirtPosition = new Vector2Int(x, y);
        transform.localPosition = new Vector3(x, 0, y);

        _meshIndex = _mesh.sharedMesh.GetBlendShapeIndex("Digged");
    }

    public void PlacePlant(Plant plant, System.Action onSuccess = null, Heritables heritables = null)
    {
        if (!_canPlace) return;
        if (_currentPlant != null) return;
        if (plant == null) return;

        var go = Instantiate(plant);
        _currentPlant = go;


        if (heritables != null)
        {
            _currentPlant.Health.MaxHealth = heritables.MaxHealth;
            _currentPlant.Health.MaxImmunity = heritables.MaxImmunity;
        }

        _currentPlant.transform.SetParent(_plantTransform);
        _currentPlant.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _currentPlant.OnExcavation += ClearPlant;
        _currentPlant.OnDeath += DestroyPlant;
        _currentPlant.SetBonus(_plantType);

        _canPlace = false;
        _mesh.SetBlendShapeWeight(_meshIndex, 0);
        _audioSource.PlayOneShot(_plantSound);

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
        _audioSource.PlayOneShot(_digSound);

        if (_currentPlant != null)
        {
            _currentPlant.OnExcavation?.Invoke();
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
    Default,
    Chernozems,
    Podzolic,
    Tundra
}

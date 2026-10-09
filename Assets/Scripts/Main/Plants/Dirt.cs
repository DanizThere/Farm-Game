using UnityEngine;

public class Dirt : MonoBehaviour
{
    public Plant Plant => _currentPlant;
    public Vector2Int DirtPosition => _dirtPosition;
    [SerializeField] private PlantType _plantType;
    [SerializeField] private Transform _plantTransform;
    private Vector2Int _dirtPosition;
    private Plant _currentPlant;

    public void InitializePosition(int x, int y)
    {
        _dirtPosition = new Vector2Int(x, y);
        transform.localPosition = new Vector3(x, 0, y);
    }

    public void PlacePlant(Plant plant)
    {
        if (_currentPlant != null) return;
        if (plant == null) return;

        _currentPlant = plant;

        _currentPlant.transform.SetParent(_plantTransform);
        _currentPlant.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _currentPlant.OnDestroy += DestroyPlant;
        _currentPlant.StartLife(_plantType);
    }

    public void RemovePlant()
    {
        DestroyPlant();
    }

    private void DestroyPlant()
    {
        Destroy(_currentPlant.gameObject);
        _currentPlant = null;
    }
}

public enum PlantType
{
    Chernozems,
    Podzolic,
    Tundra
}

using UnityEngine;

public class Dirt : MonoBehaviour
{
    public Vector2Int DirtPosition => _dirtPosition;
    [SerializeField] private PlantType _plantType;
    private Plant _plant;
    private Vector2Int _dirtPosition;

    public void InitializePosition(int x, int y)
    {
        _dirtPosition = new Vector2Int(x, y);
    }

    public void PlacePlant(Plant plant)
    {
        if (_plant != null) return;
        _plant = plant;

        _plant.OnDestroy += DestroyPlant;
        plant.StartLife(_plantType);
    }

    public void RemovePlant()
    {
        DestroyPlant();
    }

    private void DestroyPlant()
    {
        Destroy(_plant.gameObject);
        _plant = null;
    }
}

public enum PlantType
{
    Chernozems,
    Podzolic,
    Tundra
}

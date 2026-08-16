using UnityEngine;

public class Dirt : MonoBehaviour
{
    public Observable<Plant> Plant;
    public Vector2Int DirtPosition => _dirtPosition;
    [SerializeField] private PlantType _plantType;
    private Vector2Int _dirtPosition;

    public void InitializePosition(int x, int y)
    {
        _dirtPosition = new Vector2Int(x, y);
        transform.localPosition = new Vector3(x, 0, y);

        Plant = new(null);
        Plant.ValueChanged += PlacePlant;
    }

    private void PlacePlant(Plant plant)
    {
        if (Plant != null) return;

        if (plant == null) return;

        plant.OnDestroy += DestroyPlant;
        plant.StartLife(_plantType);
    }

    public void RemovePlant()
    {
        DestroyPlant();
    }

    private void DestroyPlant()
    {
        Destroy(Plant.Value.gameObject);
        Plant.Value = null;
    }
}

public enum PlantType
{
    Chernozems,
    Podzolic,
    Tundra
}

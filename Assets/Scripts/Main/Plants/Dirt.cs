using UnityEngine;

public class Dirt : MonoBehaviour
{
    [SerializeField] private PlantType _plantType;
    private Plant _plant;

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

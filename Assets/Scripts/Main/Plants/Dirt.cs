using System;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    public Plant Plant => _currentPlant;
    public Vector2Int DirtPosition => _dirtPosition;
    [SerializeField] private PlantType _plantType;
    [SerializeField] private Transform _plantTransform;
    private Vector2Int _dirtPosition;
    private Plant _currentPlant;
    private bool _canPlace = true;

    public void InitializePosition(int x, int y)
    {
        _dirtPosition = new Vector2Int(x, y);
        transform.localPosition = new Vector3(x, 0, y);
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

        _currentPlant.OnDestroy += DestroyPlant;
        _currentPlant.StartLife(_plantType);

        _canPlace = false;
        onSuccess?.Invoke();
    }

    private void DestroyPlant()
    {
        Destroy(_currentPlant.gameObject);
        print("destroy!");
        _currentPlant = null;
    }

    public void Dig(Inventory inventory)
    {
        //here another plants should react
        
        if(_currentPlant != null)
        {
            if(inventory.AddItem(_currentPlant.InitItem,
                UnityEngine.Random.onUnitCircle))
            {
                DestroyPlant();
                return;
            }
        }

        print("i dig a hole");
        _canPlace = true;
    }

    public void TryEffort(ItemObject item)
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

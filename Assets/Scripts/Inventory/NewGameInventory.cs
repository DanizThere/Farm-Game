using UnityEngine;

public class NewGameInventory : MonoBehaviour
{
    [SerializeField] private NewGameItem[] _items;
    [SerializeField] private Inventory _inventory;


    public void Initialize()
    {
        for(int i = 0; i  < _items.Length; i++)
        {
            var newPosition = Vector3.one;
            newPosition.x *= i;

            _inventory.AddItem(_items[i].Item, newPosition);
        }
    }
}

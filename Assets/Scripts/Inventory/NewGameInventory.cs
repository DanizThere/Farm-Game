using UnityEngine;

public class NewGameInventory : MonoBehaviour
{
    [SerializeField] private NewGameItem[] _items;
    [SerializeField] private Inventory _inventory;


    public void Initialize()
    {
        foreach(var item in _items)
        {
            _inventory.AddItem(item.Item);
        }
    }
}

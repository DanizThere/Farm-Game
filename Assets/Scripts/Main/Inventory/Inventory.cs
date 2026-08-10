using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemObject> _items = new();

    public void Add(ItemObject item)
    {
        //check if stackable
        _items.Add(item);
    }

    public void Remove(ItemObject item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour, IService
{
    [SerializeField] private List<Item> _items = new();

    public Item GetById(string id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        return item;
    }
}

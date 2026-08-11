using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Item Item;
    public int Amount;

    public bool IsEmpty => Item == null && Amount <= 0;

    public InventorySlot()
    {
        Item = null;
        Amount = 0;
    }

    public InventorySlot(Item item, int amount)
    {
        Item = item; 
        Amount = amount;
    }

    public int AddItems(int count)
    {
        if (Item == null) return count;

        var space = Item.MaxStack - Amount;
        var added = Mathf.Min(space, count);

        Amount += added;
        return count - added;
    }

    public bool CanPlaceInSlot(Item item, int count = 1)
    {
        if (IsEmpty) return true;

        return item == Item && Amount + count <= Item.MaxStack;
    }
}

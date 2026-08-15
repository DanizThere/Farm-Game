using NUnit.Framework.Interfaces;
using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Observable<Item> Item;
    public int Amount;
    public float Durability;

    public bool IsEmpty => Item.Value == null && Amount <= 0;

    public InventorySlot()
    {
        Item = new(null);
        Amount = 0;
        Durability = 0;

        Item.ValueChanged += value =>
        {
            if (value is ToolItem toolValue)
            {
                Durability = toolValue.Durability;
            }
        };
    }

    public InventorySlot(ToolItem data, int count, float dur = -1)
    {
        Item = new(data);
        Amount = count;
        if (data is ToolItem toolData)
        {
            Durability = (dur >= 0) ? dur : toolData.Durability;
        }
        else
        {
            Durability = 0;
        }
        Item.ValueChanged += value =>
        {
            if (value is ToolItem toolValue)
            {
                Durability = toolValue.Durability;
            }
        };
    }

    public int AddItems(int count)
    {
        if (Item == null) return count;

        var space = Item.Value.MaxStack - Amount;
        var added = Mathf.Min(space, count);

        Amount += added;
        return count - added;
    }

    public bool CanPlaceInSlot(Item item, int count = 1)
    {
        if (IsEmpty) return true;

        return item == Item.Value && Amount + count <= Item.Value.MaxStack;
    }
}

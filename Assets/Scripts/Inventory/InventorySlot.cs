using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Item Item;
    public float Weight;
    public float Durability;
    public bool IsEmpty => Item == null;
    public Vector2 Position;
    public string InstanceId;

    public InventorySlot()
    {
        Weight = 0;
        Durability = 0;
    }

    public InventorySlot(Item data, Vector2 position, float dur = -1)
    {
        Item = data;
        Weight = Item.Weight;
        SetPosition(position);
        if (data is ToolItem toolData)
        {
            Durability = (dur >= 0) ? dur : toolData.Durability;
        }
        else
        {
            Durability = 0;
        }
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
}

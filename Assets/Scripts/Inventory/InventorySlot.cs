using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Item Item;
    public int Amount;
    public float Durability;
    public bool IsEmpty => Item == null && Amount <= 0;
    public Vector3 Position;

    public InventorySlot()
    {
        Amount = 0;
        Durability = 0;
    }

    public InventorySlot(Item data, Vector3 position, float dur = -1)
    {
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

    public void SetPosition(Vector3 position)
    {
        Position = position;
    }
}

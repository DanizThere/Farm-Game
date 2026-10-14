using System;
using UnityEngine;

[Serializable]
public class InventoryHandSlot
{
    public string Hand;
    public Item Item;
    public float Durability;

    public InventoryHandSlot(Item data, string hand, float dur = -1)
    {
        Hand = hand;
        Item = data;
        if (data is ToolItem toolData)
        {
            Durability = (dur >= 0) ? dur : toolData.Durability;
        }
        else
        {
            Durability = 0;
        }
    }
}

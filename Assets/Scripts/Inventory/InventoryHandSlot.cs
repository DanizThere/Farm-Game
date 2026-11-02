using System;
using UnityEngine;

[Serializable]
public class InventoryHandSlot
{
    public string Hand;
    public Item Item;
    public float Durability;
    public string InstanceId;

    public InventoryHandSlot(Item data, string hand, string instanceId = null, float dur = -1)
    {
        InstanceId = instanceId;
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

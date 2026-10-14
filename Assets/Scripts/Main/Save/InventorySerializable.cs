using System;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class InventorySerializable
{
    public List<InventoryItemSerialize> Slots = new();
    public List<InventorySlotItemSerialize> HandSlots = new();
}

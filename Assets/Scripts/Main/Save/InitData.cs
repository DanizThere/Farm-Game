using System;

[Serializable]
public class InitData
{
    public InventorySerializable Inventory = new();
    public TimeSettings Time;
    public ChestsSerialize Chests = new();
}
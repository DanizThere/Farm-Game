using System;
using System.Collections.Generic;

[Serializable]
public class ChestsSerialize
{
    public List<ChestSerialize> Chests = new();
}

[Serializable]
public class ChestSerialize
{
    public string Id;
    public List<InventoryItemSerialize> Items = new();
}
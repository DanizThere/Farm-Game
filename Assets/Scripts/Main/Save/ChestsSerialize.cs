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
    public Vector3Serialize Position;
    public QuaternionSerialize Rotation;
    public List<InventoryItemSerialize> Items = new();
}
using System;
using UnityEngine;

[Serializable]
public class InventoryItemSerialize
{
    public string Id;
    public float Weight;
    public float Durability;
    public Vector2Serialize Position;
}

[Serializable]
public class Vector2Serialize
{
    public float X, Y;
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "Items/Tool")]
public class ToolItem : Item
{
    public float MaxDurability = 10f;
    public float Durability = 10f;
}

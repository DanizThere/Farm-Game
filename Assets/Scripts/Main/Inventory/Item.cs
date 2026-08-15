using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string Id;
    public int MaxStack = 4;
    public GameObject Prefab;
    public Sprite Icon;
}

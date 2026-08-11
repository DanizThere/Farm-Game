using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string Id = Guid.NewGuid().ToString();
    public bool IsStackable = true;
    public int MaxStack = 4;
    public GameObject Prefab;
    public Sprite Icon;
}

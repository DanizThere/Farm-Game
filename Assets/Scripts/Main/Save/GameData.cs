using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public InventorySerializable Inventory = new();
    public TimeSerialize Time = new();
    public List<SaveObject> Objects = new();
    public DialogueSerialize Dialogues = new();
    public PlayerData Player = new();
}

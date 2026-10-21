using UnityEngine;

[System.Serializable]
public class GameData
{
    public InventorySerializable Inventory = new();
    public TimeSerialize Time = new();
    public ChestsSerialize Chests = new();
    public DialogueSerialize Dialogues = new();
}

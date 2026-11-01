using System;
using System.Collections.Generic;

[Serializable]
public class InitData
{
    public List<InventoryStart> Inventory = new();
    public TimeSettings Time;
    public List<SaveObject> Objects = new();
    public List<DialogueNode> Dialogues = new();
    public PlayerData Player = new();
}
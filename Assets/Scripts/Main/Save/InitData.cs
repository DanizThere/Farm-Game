using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InitData
{
    public List<InventoryStart> Inventory = new();
    public TimeSettings Time;
    public List<SaveObject> Objects = new();
    public List<DialogueNode> Dialogues = new();
    public PlayerData Player = new();
    public Vector3 PlayerStartPosition;
}
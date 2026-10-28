using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameDbContext : IService
{
    public ItemSet<ItemEffortHandler> ItemEffortHandlers = new();
    public ItemSet<ItemObject> KeyItemObjectValue = new();
    public ItemSet<Item> Items = new();
    public ItemSet<DialogueNode> DialogueNodes = new();
    public ItemSet<PlantHealthCharacteristicData> PlantHealthCharacteristics = new();

    public ItemSet<DialogueNode> CurrentNodes = new();
    public void AddNodes(List<string> nodes)
    {
        foreach(var id in nodes)
        {
            var node = DialogueNodes.Get(x => x.Id == id).FirstOrDefault();
            if(node != null && !CurrentNodes.Contains(node))
            {
                CurrentNodes.Add(node);
            }
        }
    }

    public void SaveNodes(ref List<string> ids)
    {
        ids = CurrentNodes.Get().Select(x => x.Id).ToList();
    }
}

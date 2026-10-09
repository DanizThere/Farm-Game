using UnityEngine;

[System.Serializable]
public class GameDbContext : IService
{
    public ItemSet<ItemEffortHandler> ItemEffortHandlers = new();
    public ItemSet<KeyItemObjectValue> KeyItemObjectValue = new();
}

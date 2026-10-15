using Assets.Scripts.Extenisions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestService : IService
{
    private ChestsSerialize _chests = new();

    public void LoadChests()
    {
        ServiceLocator.Instance.GetService<JsonSaveService>().Load<GameData>(nameof(GameData), data =>
        {
            _chests = data.Chests;
        });
    }

    public void SaveChests()
    {
        ServiceLocator.Instance.GetService<JsonSaveService>().Load<GameData>(nameof(GameData), data =>
        {
            data.Chests = _chests;
            ServiceLocator.Instance.GetService<JsonSaveService>().Save(nameof(GameData), data);
        });
    }

    public ChestSerialize GetChest(string id)
    {
        return _chests.Chests.FirstOrDefault(x => x.Id == id);
    }

    public void Put(List<InventorySlot> slots, string id)
    {
        var chest = _chests.Chests.FirstOrDefault(x => x.Id == id);
        if (chest != null)
        {
            _chests.Chests.Remove(chest);
        }

        var chestSerialize = new ChestSerialize
        {
            Id = id,
            Items = slots.Select(x => new InventoryItemSerialize
            {
                Id = x.Item.Id,
                Durability = x.Durability,
                Position = x.Position.ToSerializable(),
                Weight = x.Weight
            }).ToList()
        };

        _chests.Chests.Add(chestSerialize);
    }
}

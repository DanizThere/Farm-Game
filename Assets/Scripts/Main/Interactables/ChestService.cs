using Assets.Scripts.Extenisions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestService : IService
{
    public Chest _basePrefab;
    private ChestsSerialize _chests = new();

    public void Load(ChestsSerialize chests)
    {
        var existedChests = GameObject.FindObjectsByType<Chest>();
        foreach(var info in chests.Chests)
        {
            var chest = existedChests.FirstOrDefault(x => x.Id == info.Id);
            if (info != null)
            {
                chest.LoadItems(info);
                continue;
            }

            var instance = GameObject.Instantiate(_basePrefab);
            instance.transform.position = info.Position.ToVector();
            instance.transform.rotation = info.Rotation.Deserialize();
            instance.LoadItems(info);
        }

        _chests = chests;
    }

    public void Save(ref ChestsSerialize chests)
    {
        chests = _chests;
    }

    public void Put(List<InventorySlot> slots, string id, Vector3 position, Quaternion rotation)
    {
        var chest = _chests.Chests.FirstOrDefault(x => x.Id == id);
        if (chest != null)
        {
            _chests.Chests.Remove(chest);
        }

        var chestSerialize = new ChestSerialize
        {
            Id = id,
            Rotation = rotation.ToSerialize(),
            Position = position.ToSerializable(),
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

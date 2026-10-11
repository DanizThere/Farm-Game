using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public float SlotsCount => _slots.Count;
    [SerializeField] private float _maxWeight = 200f;
    [SerializeField] private List<InventorySlot> _slots = new();
    private float _currentWeight = 0f;

    public Action OnInventoryChanged = delegate { };

    public InventorySerializable SaveData()
    {
        var saveData = new InventorySerializable();

        foreach (var slot in _slots)
        {
            var slotData = new InventoryItemSerialize
            {
                Id = slot.Item.Value.Id,
                Weight = slot.Item.Value.Weight,
                Durability = slot.Durability
            };

            saveData.Slots.Add(slotData);
        }

        return saveData;
    }

    public void AddItem(Item item)
    {
        if (item == null) return;

        var newWeight = _currentWeight + item.Weight;
        if (newWeight > _maxWeight) return;

        var slot = new InventorySlot(item);
        _slots.Add(slot);
        _currentWeight = newWeight;

        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Count) return;
        var slot = _slots[slotIndex];
        if (slot.IsEmpty) return;

        if (slot.Amount <= 0)
        {
            slot.Item.Value = null;
            slot.Amount = 0;
            slot.Durability = 0;
        }
        OnInventoryChanged?.Invoke();
    }

    public InventorySlot GetSlot(int index)
    {
        return _slots[index];
    }

    public void Load(InventorySerializable data)
    {
        var itemDatabase = ServiceLocator.Instance.GetService<GameDbContext>();

        foreach(var slot in data.Slots)
        {
            _slots.Add(new InventorySlot());
        }

        for(int i = 0; i < _slots.Count; i++)
        {
            var slotData = data.Slots[i];
            if (string.IsNullOrEmpty(slotData.Id))
            {
                _slots[i] = new InventorySlot();
            }
            else
            {
                var item = itemDatabase.Items.Get(x => x.Id == slotData.Id).FirstOrDefault();
                if(item != null)
                {
                    _slots[i] = new InventorySlot(item, slotData.Durability);
                }
                else
                {
                    Debug.Log($"Item with {slotData.Id} not found");
                    _slots[i] = new InventorySlot();
                }
            }
        }

        OnInventoryChanged?.Invoke();
    }

    public void ChangeItemWithDurability(int slotIndex, float volume)
    {
        var slot = GetSlot(slotIndex);
        slot.Durability = volume;
    }
}

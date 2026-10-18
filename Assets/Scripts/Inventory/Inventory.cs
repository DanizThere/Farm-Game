using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public float CurrentWeight => _currentWeight;
    public float MaxWeight => _maxWeight;

    public float SlotsCount => _slots.Count;
    public float HandSlotsCount => _handSlots.Count;
    [SerializeField] private float _maxWeight = 200f;
    [SerializeField] private List<InventorySlot> _slots = new();
    [SerializeField] private List<InventoryHandSlot> _handSlots = new();
    private float _currentWeight = 0f;

    public Action OnInventoryChanged = delegate { };

    public void Save(InventorySerializable target)
    {

        foreach (var slot in _slots)
        {
            if (slot.IsEmpty) continue;

            var slotData = new InventoryItemSerialize
            {
                Id = slot.Item.Id,
                Weight = slot.Item.Weight,
                Durability = slot.Durability,
                Position = new Vector2Serialize
                {
                    X = slot.Position.x,
                    Y = slot.Position.y
                },
            };

            target.Slots.Add(slotData);
        }

        foreach(var slot in _handSlots)
        {
            if (slot == null) continue;

            var slotData = new InventorySlotItemSerialize
            {
                Id = slot.Item.Id,
                Durability = slot.Durability,
                Hand = slot.Hand,
            };

            target.HandSlots.Add(slotData);
        }
    }

    public bool AddItem(Item item, Vector2 position)
    {
        if (item == null) return false;

        var newWeight = _currentWeight + item.Weight;
        if (newWeight > _maxWeight) return false;

        var slot = new InventorySlot(item, position);
        _slots.Add(slot);
        _currentWeight = newWeight;

        OnInventoryChanged?.Invoke();
        return true;
    }

    public void AddItemToHandSlot(Item item, string hand, float durability = -1)
    {
        if(item == null) return;

        var slot = new InventoryHandSlot(item, hand, durability);
        _handSlots.Add(slot);

        OnInventoryChanged?.Invoke();
    }

    public void RemoveItemFromHand(string hand)
    {
        var slot = _handSlots.FirstOrDefault(x => x.Hand == hand);
        if (slot != null)
        {
            var handSlot = FindObjectsByType<HandSlot>();
            foreach(var slotObject in handSlot)
            {
                slotObject.TryClearItem(hand);
            }
            _handSlots.Remove(slot);

            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Count) return;
        var slot = _slots[slotIndex];
        if (slot.IsEmpty) return;

        _slots.Remove(slot);
        _currentWeight -= slot.Weight;

        OnInventoryChanged?.Invoke();
    }

    public InventorySlot GetSlot(int index)
    {
        if (index >= _slots.Count) return null;

        return _slots[index];
    }

    public InventoryHandSlot GetHandSlot(string hand)
    {
        return _handSlots.FirstOrDefault(x => x.Hand == hand);
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
                    var vector2 = new Vector2(slotData.Position.X, slotData.Position.Y);
                    _slots[i] = new InventorySlot(item, vector2, slotData.Durability);
                    _currentWeight += slotData.Weight;
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

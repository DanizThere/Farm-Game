using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int SlotsCount => _slots.Count;
    public int QuickSlots => _quickSlots > _slotsCount ? 4 : _quickSlots;
    [SerializeField] private int _slotsCount = 12;
    [SerializeField] private int _quickSlots = 4;
    [SerializeField] private List<InventorySlot> _slots;

    public Action OnInventoryChanged = delegate { };

    public void InitializeSlots()
    {
        _slots = new(_slotsCount);
        for (int i = 0; i < _slotsCount; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    public InventorySerializable SaveData()
    {
        ServiceLocator.Instance.GetService<ToolController>().CurrentTool?.Hide();

        var saveData = new InventorySerializable
        {
            SlotsCount = _slotsCount
        };

        foreach (var slot in _slots)
        {
            var slotData = new InventoryItemSerialize();
            if (slot.IsEmpty)
            {
                slotData.Id = string.Empty;
                slotData.Count = 0;
                slotData.Durability = 0f;
            }
            else
            {
                slotData.Id = slot.Item.Value.Id;
                slotData.Count = slot.Amount;
                slotData.Durability = slot.Durability;
            }

            saveData.Slots.Add(slotData);
        }

        return saveData;
    }

    public void ChangeItemWithDurability(int slotIndex, float durability)
    {
        var slot = GetSlot(slotIndex);

        slot.Durability = durability;
    }

    public int AddItemWithDurability(ToolItem item, int count, float durability)
    {
        if (item == null || count <= 0) return count;

        if (item is ToolItem)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].IsEmpty)
                {
                    _slots[i] = new InventorySlot(item, count, durability);
                    OnInventoryChanged?.Invoke();
                    return 0;
                }
            }
            return count;
        }
        else
        {
            return AddItem(item, count);
        }
    }

    public int AddItem(Item item, int count)
    {
        if (item == null || count <= 0) return count;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (!_slots[i].IsEmpty && _slots[i].Item == item)
            {
                var remaining = _slots[i].AddItems(count);
                if (remaining == 0)
                {
                    OnInventoryChanged?.Invoke();
                    return 0;
                }
                count = remaining;
            }
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].IsEmpty)
            {
                var toPlace = Mathf.Min(count, item.MaxStack);
                _slots[i].Item.Value = item;
                _slots[i].Amount = toPlace;
                count -= toPlace;
                if (count == 0)
                {
                    OnInventoryChanged?.Invoke();
                    return 0;
                }
            }
        }

        OnInventoryChanged?.Invoke();
        return count;
    }

    public void RemoveItem(int slotIndex, int count)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Count) return;
        var slot = _slots[slotIndex];
        if (slot.IsEmpty) return;

        slot.Amount -= count;
        if (slot.Amount <= 0)
        {
            slot.Item.Value = null;
            slot.Amount = 0;
            slot.Durability = 0;
        }
        OnInventoryChanged?.Invoke();
    }

    public void MoveItem(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex) return;
        var fromSlot = _slots[fromIndex];
        var toSlot = _slots[toIndex];

        if (toSlot.IsEmpty)
        {
            toSlot.Item.Value = fromSlot.Item.Value;
            toSlot.Amount = fromSlot.Amount;
            toSlot.Durability = fromSlot.Durability;
            fromSlot.Item.Value = null;
            fromSlot.Amount = 0;
            fromSlot.Durability = 0;

            _slots[fromIndex] = fromSlot;
            _slots[toIndex] = toSlot;

            OnInventoryChanged?.Invoke();
            return;
        }

        if (fromSlot.Item == toSlot.Item)
        {
            var space = toSlot.Item.Value.MaxStack - toSlot.Amount;
            var moveCount = Mathf.Min(space, fromSlot.Amount);
            toSlot.Amount += moveCount;
            fromSlot.Amount -= moveCount;
            if (fromSlot.Amount <= 0)
            {
                fromSlot.Item.Value = null;
                fromSlot.Amount = 0;
                _slots[fromIndex] = fromSlot;
            }
            OnInventoryChanged?.Invoke();
            return;
        }

        var tempData = toSlot.Item.Value;
        var tempAmount = toSlot.Amount;
        var tempDurability = toSlot.Durability;
        toSlot.Item.Value = fromSlot.Item.Value;
        toSlot.Amount = fromSlot.Amount;
        toSlot.Durability = fromSlot.Durability;
        fromSlot.Item.Value = tempData;
        fromSlot.Amount = tempAmount;
        fromSlot.Durability = tempDurability;

        _slots[fromIndex] = fromSlot;
        _slots[toIndex] = toSlot;

        OnInventoryChanged?.Invoke();
    }

    public InventorySlot GetSlot(int index)
    {
        return _slots[index];
    }

    public void Load(InventorySerializable data)
    {
        InitializeSlots();

        var itemDatabase = ServiceLocator.Instance.GetService<ItemDatabase>();

        while (_slots.Count < data.SlotsCount)
        {
            _slots.Add(new InventorySlot());
        }
        if(_slots.Count > data.SlotsCount)
        {
            _slots.RemoveRange(data.SlotsCount, _slots.Count - data.SlotsCount);
        }

        for(int i = 0; i < Mathf.Min(_slots.Count, data.Slots.Count); i++)
        {
            var slotData = data.Slots[i];
            if (string.IsNullOrEmpty(slotData.Id))
            {
                _slots[i] = new InventorySlot();
            }
            else
            {
                var item = itemDatabase.GetById(slotData.Id);
                if(item != null)
                {
                    _slots[i] = new InventorySlot(item, slotData.Count, slotData.Durability);
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
}

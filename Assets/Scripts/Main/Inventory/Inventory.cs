using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int SlotsCount => _slots.Count;

    [SerializeField] private int _slotsCount = 13;
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
                _slots[i].Item = item;
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
            slot.Item = null;
            slot.Amount = 0;
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
            toSlot.Item = fromSlot.Item;
            toSlot.Amount = fromSlot.Amount;
            fromSlot.Item = null;
            fromSlot.Amount = 0;
            OnInventoryChanged?.Invoke();
            return;
        }

        if (fromSlot.Item == toSlot.Item)
        {
            var space = toSlot.Item.MaxStack - toSlot.Amount;
            var moveCount = Mathf.Min(space, fromSlot.Amount);
            toSlot.Amount += moveCount;
            fromSlot.Amount -= moveCount;
            if (fromSlot.Amount <= 0)
            {
                fromSlot.Item = null;
                fromSlot.Amount = 0;
            }
            OnInventoryChanged?.Invoke();
            return;
        }

        var tempData = toSlot.Item;
        var tempAmount = toSlot.Amount;
        toSlot.Item = fromSlot.Item;
        toSlot.Amount = fromSlot.Amount;
        fromSlot.Item = tempData;
        fromSlot.Amount = tempAmount;
        OnInventoryChanged?.Invoke();
    }

    public InventorySlot GetSlot(int index)
    {
        return _slots[index];
    }
}

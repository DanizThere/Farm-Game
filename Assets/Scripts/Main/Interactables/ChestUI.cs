using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestUI : View, IDropHandler
{
    public event Action OnClose;
    [SerializeField] private GameObject _panel;
    private List<InventorySlotUI> _slotsUI = new();
    private Inventory _inventory;


    private void Awake()
    {
        _isActive.ValueChanged += _panel.SetActive;
    }

    private void OnDestroy()
    {
        OnClose?.Invoke();
    }

    public override void Hide()
    {
        base.Hide();
        OnClose?.Invoke();

        foreach(var slot in _slotsUI)
        {
            Destroy(slot.gameObject);
        }

        _slotsUI.Clear();
    }

    public override void Show()
    {
        base.Show();

        var invSlotUI = ServiceLocator.Instance.GetService<ViewController>().Get<InventorySlotUI>();

        for (int i = 0; i < _inventory.SlotsCount; i++)
        {
            var slot = Instantiate(invSlotUI, _panel.transform);
            slot.Initialize(_inventory, i);
            _slotsUI.Add(slot);

            slot.UpdateSlot();
        }
    }

    public InventorySlot GetSlot(int index)
    {
        return _inventory.GetSlot(index);
    }

    public void Init(Inventory inventory)
    {
        _inventory = inventory;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<InventorySlotUI>(out var ui))
        {
            var slotItem = ui.GetSlotByIndex();
            if (slotItem == null) return;

            var point = ui.ScreenToLocal(_panel.transform as RectTransform, eventData.position, eventData.pressEventCamera);

            if (eventData.pointerDrag.TryGetComponent<HandSlotUI>(out var slot))
            {
                var slotHandItem = slot.GetSlot();
                if (slotItem == null) return;
                if (_inventory.AddItem(slotHandItem.Item, eventData.position, slotHandItem.InstanceId))
                    slot.ClearItem();
            }
            if (ui.InventorySource != _inventory)
            {
                _inventory.AddItem(slotItem.Item, point, slotItem.InstanceId);
                ui.InventorySource.RemoveItem(ui.SlotIndex);
            }
            else
            {
                slotItem.SetPosition(point);
            }

            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        foreach (var slot in _slotsUI)
        {
            Destroy(slot.gameObject);
        }

        _slotsUI.Clear();
        var invSlotUI = ServiceLocator.Instance.GetService<ViewController>().Get<InventorySlotUI>();

        for (int i = 0; i < _inventory.SlotsCount; i++)
        {
            var slot = Instantiate(invSlotUI, _panel.transform);
            slot.Initialize(_inventory, i);
            _slotsUI.Add(slot);

            slot.UpdateSlot();
        }
    }
}
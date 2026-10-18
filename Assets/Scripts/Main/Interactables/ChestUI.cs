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

            var invSlot = _inventory.GetSlot(ui.SlotIndex);
            if(invSlot != slotItem)
                _inventory.AddItem(slotItem.Item, eventData.position);
            else
                slotItem.SetPosition(eventData.position);
        }
    }
}
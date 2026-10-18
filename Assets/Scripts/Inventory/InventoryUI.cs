using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : View, IDropHandler
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _weight;

    private Inventory _inventory;
    private List<InventorySlotUI> _slotsUI = new();
    private List<HandSlotUI> _handSlotsUI = new();

    private List<HandSlot> _hands = new();

    private void Awake()
    {
        _isActive.ValueChanged += _panel.SetActive;
    }

    public void Setup(Inventory inventory, List<HandSlot> hands)
    {
        _inventory = inventory;
        _hands = hands;

        CreateSlots();

        _inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDestroy()
    {
        foreach(var slot in _handSlotsUI)
        {
            Destroy(slot.gameObject);
        }

        if(_inventory != null)
            _inventory.OnInventoryChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        foreach(var slot in _slotsUI)
        {
            Destroy(slot.gameObject);
        }
        foreach(var slot in _handSlotsUI)
        {
            Destroy(slot.gameObject);
        }

        _slotsUI.Clear();
        _handSlotsUI.Clear();

        CreateSlots();
    }

    public override void Show()
    {
        _isActive.Value = true;
    }

    public override void Hide()
    {
        _isActive.Value = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.TryGetComponent<HandSlotUI>(out var slot))
            {
                var slotItem = slot.GetSlot();
                if (slotItem == null) return;
                if(_inventory.AddItem(slotItem.Item, eventData.position))
                    _inventory.RemoveItemFromHand(slotItem.Hand);
            }

            if (eventData.pointerDrag.TryGetComponent<InventorySlotUI>(out var ui))
            {
                var slotItem = ui.GetSlotByIndex();
                if (slotItem == null) return;

                slotItem.SetPosition(eventData.position);
            }
        }
    }

    private void CreateSlots()
    {
        var invSlotUI = ServiceLocator.Instance.GetService<ViewController>().Get<InventorySlotUI>();
        var hndSlotUI = ServiceLocator.Instance.GetService<ViewController>().Get<HandSlotUI>();
        var hndParent = ServiceLocator.Instance.GetService<ViewController>().HandsSlotTransform;

        for (int i = 0; i < _inventory.SlotsCount; i++)
        {
            var slot = Instantiate(invSlotUI, _panel.transform);
            slot.Initialize(_inventory, i);
            _slotsUI.Add(slot);

            slot.UpdateSlot();
        }

        for (int i = 0; i < _hands.Count; i++)
        {
            var slot = Instantiate(hndSlotUI, hndParent);
            slot.Initialize(_inventory, _hands[i]);
            _handSlotsUI.Add(slot);

            slot.UpdateSlot();
        }

        _weight.text = $"Weight: {_inventory.CurrentWeight} / {_inventory.MaxWeight} (max)";
    }
}

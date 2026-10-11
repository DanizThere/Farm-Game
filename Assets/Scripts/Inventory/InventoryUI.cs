using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : View
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameObject _panel;

    private Inventory _inventory;
    private List<InventorySlotUI> _slotsUI = new();

    private void Awake()
    {
        _isActive.ValueChanged += _panel.SetActive;
    }

    public void Setup(Inventory inventory)
    {
        _inventory = inventory;
        _slotsUI = new();
        for (int i = 0; i < _inventory.SlotsCount; i++)
        {
            var slotGO = Instantiate(_slotPrefab, _panel.transform);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(_inventory, i);
            _slotsUI.Add(slotUI);
        }

        _inventory.OnInventoryChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        foreach(var slot in _slotsUI)
        {
            slot.UpdateSlot();
        }
    }

    public override void Show()
    {
        _isActive.Value = true;
    }

    public override void Hide()
    {
        _isActive.Value = false;
    }
}

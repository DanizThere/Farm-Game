using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IView
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotsParent;
    [SerializeField] private GameObject _panel;

    private Inventory _inventory;
    private List<InventorySlotUI> _slotsUI = new();

    public int Order { get; set; } = 10;
    public bool IsActive { get; set; }

    public void Setup(Inventory inventory)
    {
        _inventory = inventory;
        _slotsUI = new(_inventory.SlotsCount);
        for (int i = 0; i < _inventory.SlotsCount; i++)
        {
            var slotGO = Instantiate(_slotPrefab, _slotsParent);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(_inventory, i);
            _slotsUI.Add(slotUI);
        }

        _inventory.OnInventoryChanged += RefreshUI;

        ServiceLocator.Instance.GetService<ViewController>().Add(this);
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

    public void Show()
    {
        IsActive = true;
        _panel.SetActive(true);
    }

    public void Hide()
    {
        IsActive = false;
        _panel.SetActive(false);
    }
}

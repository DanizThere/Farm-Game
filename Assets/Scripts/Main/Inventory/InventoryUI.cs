using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotsParent;
    [SerializeField] private GameObject _panel;

    private Inventory _inventory;
    private List<InventorySlotUI> _slotsUI = new();

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
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryChanged -= RefreshUI;
    }

    public void SetActive(bool active)
    {
        _panel.SetActive(active);
    }

    private void RefreshUI()
    {
        foreach(var slot in _slotsUI)
        {
            slot.UpdateSlot();
        }
    }

}

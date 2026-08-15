using System.Collections.Generic;
using UnityEngine;

public class InventoryUIQuickSlots : MonoBehaviour, IView
{
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    [SerializeField] private Transform _parent;
    [SerializeField] private InventoryUIQuickSlot _prefab;

    private PlayerController _playerController;
    private List<InventoryUIQuickSlot> _quickBar = new();
    private Inventory _inventory;

    private Observable<InventoryUIQuickSlot> _currentSlot = new(null);

    public void Hide()
    {
        IsActive = false;
        _parent.gameObject.SetActive(IsActive);
    }

    public void Setup(Inventory inventory)
    {
        _inventory = inventory;
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();

        ServiceLocator.Instance.GetService<ViewController>().Add(this);

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnStartEvent += Hide;
        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>().OnStartEvent += Show;

        _playerController.OnDigitClickEvent += TryGetSlot;

        for (int i = 0; i < inventory.QuickSlots; i++)
        {
            var slotGO = Instantiate(_prefab, _parent);
            var slotUI = slotGO.GetComponent<InventoryUIQuickSlot>();
            slotUI.Initialize(_inventory, i);
            slotUI.HideOutline();
            _quickBar.Add(slotUI);
        }

        _inventory.OnInventoryChanged += RefreshUI;
        _currentSlot.ValueChanged += value =>
        {
            foreach (var slot in _quickBar)
            {
                slot.HideOutline();
            }
            value?.ShowOutline();
        };
    }

    public void Show()
    {
        IsActive = true;
        _parent.gameObject.SetActive(IsActive);
    }

    private void RefreshUI()
    {
        foreach (var slot in _quickBar)
        {
            slot.UpdateSlot();
        }
    }

    public void TryGetSlot(int slot)
    {
        var inventorySlot = _quickBar[slot];
        _currentSlot.Value = inventorySlot;
    }
}

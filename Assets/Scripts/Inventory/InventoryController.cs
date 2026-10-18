using System.Collections.Generic;
using UnityEngine;

public class InventoryController : SLMonoBehaviour, IService
{
    public Inventory Inventory => _inventory;
    [SerializeField] private Inventory _inventory;
    private List<HandSlot> _slots = new();
    private InventoryUI _inventoryUI;

    private GameStateMachine _gameStateMachine;
    private ViewController _viewController;

    protected override void SLStart()
    {
        _gameStateMachine = ServiceLocator.Instance.GetService<GameStateMachine>() ??
            throw new System.NullReferenceException($"{nameof(GameStateMachine)} doens't exist");
        _viewController = ServiceLocator.Instance.GetService<ViewController>() ??
            throw new System.NullReferenceException($"{nameof(ViewController)} doens't exist");

        _gameStateMachine.GetState<OuterWorldGameState>().OnStartEvent += HideUI;
        _gameStateMachine.GetState<UIGameState>().OnStartEvent += ShowUI;
    }

    public void AddSlot(HandSlot slot)
    {
        if (!_slots.Contains(slot))
        {
            _slots.Add(slot);
        }
    }

    public void Show()
    {
        _gameStateMachine.SetState<UIGameState>();
    }

    public void Hide()
    {
        _gameStateMachine.SetState<OuterWorldGameState>();
    }

    private void ShowUI()
    {
        _inventoryUI = Instantiate(_viewController.Get<InventoryUI>(), _viewController.Canvas2D.transform);
        _inventoryUI.Setup(_inventory, _slots);
    }

    private void HideUI()
    {
        Destroy(_inventoryUI.gameObject);
        _inventoryUI = null;
    }
}

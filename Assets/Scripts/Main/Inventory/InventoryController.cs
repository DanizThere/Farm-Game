using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private InventoryUIQuickSlots _inventoryUIQuick;

    [SerializeField] private Transform _3DUIParent;

    [SerializeField] private GameObject _dragObject;
    [SerializeField] private Image _dragIcon;
    [SerializeField] private TextMeshProUGUI _dragAmount;
    [SerializeField] private RectTransform _dragParent;
    private int _iconIndex;

    private PlayerController _playerController;
    private ToolController _toolController;

    private GameStateMachine _gameStateMachine;
    private UIGameState _uiState;
    private OuterWorldGameState _outerState;

    private Observable<ISelectable> _selectableObservable;

    private bool _isOpen = false;

    private bool _isSelectedSlot;

    private Observable<int> _currentIndex = new(0);
    private int _lastIndex = 3;
    private int _firstIndex = 0;

    private void Start()
    {
        _inventory.InitializeSlots();
        _inventoryUIQuick.Setup(_inventory);
        _inventoryUI.Setup(_inventory);

        _playerController = ServiceLocator.Instance.GetService<PlayerController>();
        _toolController = ServiceLocator.Instance.GetService<ToolController>();
        _gameStateMachine = ServiceLocator.Instance.GetService<GameStateMachine>();

        _uiState = _gameStateMachine.GetState<UIGameState>();
        _outerState = _gameStateMachine.GetState<OuterWorldGameState>();

        if (_uiState)
        {
            _uiState.OnStartEvent += SetupUIState;
            _uiState.OnExitEvent += DisposeUIState;
        }
        if (_outerState)
        {
            _outerState.OnStartEvent += SetupOuterState;
            _outerState.OnExitEvent += DisposeOuterState;
        }

        _gameStateMachine.SetState<OuterWorldGameState>();

        _currentIndex.ValueChanged += _inventoryUIQuick.TryGetSlot;

        _selectableObservable = new(null);
        _selectableObservable.ValueChanged += value => value.Show();

        if (_isOpen)
        {
            _inventoryUI.Show();
            return;
        }
        _inventoryUI.Hide();
    }

    private void OnDestroy()
    {
        if (_uiState)
        {
            _uiState.OnStartEvent -= SetupUIState;
            _uiState.OnExitEvent -= DisposeUIState;
        }
        if (_outerState)
        {
            _outerState.OnStartEvent -= SetupOuterState;
            _outerState.OnExitEvent -= DisposeOuterState;
        }
    }

    private void Update()
    {
        if (_isSelectedSlot)
        {
            var dragRect = _dragObject.GetComponent<RectTransform>();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle
                (_dragParent, 
                Mouse.current.position.ReadValue(), 
                null,
                out var localMousePosition))
            {
                dragRect.anchoredPosition = localMousePosition;
            }
        }
    }

    private void OnClick()
    {
        if (!_isOpen) return;

        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach(var result in results)
        {
            if (result.gameObject.TryGetComponent<InventorySlotUI>(out var slot))
            {
                if (_isSelectedSlot)
                {
                    _inventory.MoveItem(_iconIndex, slot.SlotIndex);
                    _dragObject.SetActive(false);
                    _isSelectedSlot = false;

                    if(slot.SlotIndex == _currentIndex.Value)
                    {
                        TryGetTool(slot.SlotIndex);
                    }
                    return;
                }

                var item = slot.GetSlotByIndex();
                if (item.IsEmpty) return;
                print(item);

                _isSelectedSlot = true;
                _iconIndex = slot.SlotIndex;
                _dragAmount.text = item.Amount.ToString();
                _dragIcon.sprite = item.Item.Value.Icon;
                _dragObject.SetActive(true);
            }
        }
    }

    private void TryGetTool(int slot)
    {
        var inventorySlot = _inventory.GetSlot(slot);
        if (inventorySlot.IsEmpty)
        {
            _toolController.SetTool(null);
            return;
        }

        var item = inventorySlot.Item;

        if (item.Value.Prefab == null)
        {
            _toolController.SetTool(null);
            return;
        }

        var instantTool = Instantiate(item.Value.Prefab, _toolController.transform);
        if(instantTool.TryGetComponent<Tool>(out var tool)){
            var toolItem = item.Value as ToolItem;

            var inst = Instantiate(toolItem);
            inst.Durability = inventorySlot.Durability;

            tool.Setup(_inventory, inst, _3DUIParent, slot);
            _toolController.SetTool(tool);
        }
    }

    private void IncreaseIndex()
    {
        if (_currentIndex.Value + 1 > _lastIndex)
        {
            _currentIndex.Value = _firstIndex;
            TryGetTool(_currentIndex);
            return;
        }
        _currentIndex.Value++;
        TryGetTool(_currentIndex);
    }

    private void DecreaseIndex()
    {
        if (_currentIndex.Value - 1 < _firstIndex)
        {
            _currentIndex.Value = _lastIndex;
            TryGetTool(_currentIndex);
            return;
        }
        _currentIndex.Value--;
        TryGetTool(_currentIndex);
    }

    private void ChangeVisibility()
    {
        _isOpen = !_isOpen;

        if (!_isOpen)
        {
            ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
            ResetDrag();
            _inventoryUI.Hide();
        }
        else
        {
            ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
            _inventoryUI.Show();
        }
    }

    private void ResetDrag()
    {
        _isSelectedSlot = false;
        _dragIcon.sprite = null;
        _dragAmount.text = string.Empty;
        _dragObject.SetActive(false);
    }

    private void SetupOuterState()
    {
        _playerController.OnInventoryClickEvent += ChangeVisibility;
        _playerController.OnDigitClickEvent += TryGetTool;
        _playerController.OnNextClickEvent += IncreaseIndex;
        _playerController.OnBackClickEvent += DecreaseIndex;
    }

    private void SetupUIState()
    {
        _playerController.OnInventoryClickEvent += ChangeVisibility;
        _playerController.OnClickEvent += OnClick;
    }

    private void DisposeOuterState()
    {
        _playerController.OnInventoryClickEvent -= ChangeVisibility;
        _playerController.OnDigitClickEvent -= TryGetTool;
        _playerController.OnNextClickEvent -= IncreaseIndex;
        _playerController.OnBackClickEvent -= DecreaseIndex;
    }

    private void DisposeUIState()
    {
        _playerController.OnInventoryClickEvent -= ChangeVisibility;
        _playerController.OnClickEvent -= OnClick;
    }
}

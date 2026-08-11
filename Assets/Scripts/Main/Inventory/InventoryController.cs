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

    [SerializeField] private GameObject _dragObject;
    [SerializeField] private Image _dragIcon;
    [SerializeField] private TextMeshProUGUI _dragAmount;
    [SerializeField] private RectTransform _dragParent;
    private int _iconIndex;

    private PlayerController _playerController;
    private CameraHandler _cameraHandler;
    private ToolController _toolController;

    private bool _isOpen = false;

    private bool _isSelectedSlot;

    private int _currentIndex;
    private int _lastIndex = 3;
    private int _firstIndex = 0;

    private void Start()
    {
        _inventory.InitializeSlots();
        _inventoryUI.Setup(_inventory);

        _playerController = ServiceLocator.Instance.GetService<PlayerController>();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
        _toolController = ServiceLocator.Instance.GetService<ToolController>();

        _playerController.OnClickEvent += OnClick;
        _playerController.OnInventoryClickEvent += ChangeVisibility;
        _playerController.OnDigitClickEvent += TryGetTool;
        _playerController.OnNextClickEvent += IncreaseIndex;
        _playerController.OnBackClickEvent += DecreaseIndex;

        _inventoryUI.SetActive(_isOpen);
    }

    private void OnDestroy()
    {
        _playerController.OnClickEvent -= OnClick;
        _playerController.OnInventoryClickEvent -= ChangeVisibility;
        _playerController.OnDigitClickEvent -= TryGetTool;
        _playerController.OnNextClickEvent -= IncreaseIndex;
        _playerController.OnBackClickEvent -= DecreaseIndex;
    }

    private void Update()
    {
        if (_isSelectedSlot)
        {
            var dragRect = _dragObject.GetComponent<RectTransform>();

            var localMousePosition = Vector2.zero;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle
                (_dragParent, 
                Mouse.current.position.ReadValue(), 
                _cameraHandler.Camera,
                out localMousePosition))
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
                    return;
                }

                var item = slot.GetSlotByIndex();
                if (item.IsEmpty) return;
                _isSelectedSlot = true;
                _iconIndex = slot.SlotIndex;
                _dragAmount.text = item.Amount.ToString();
                _dragIcon.sprite = item.Item.Icon;
                _dragObject.SetActive(true);
            }
        }
    }

    private void TryGetTool(int slot)
    {
        var inventorySlot = _inventory.GetSlot(slot - 1);
        if (inventorySlot.IsEmpty) return;

        var item = inventorySlot.Item;

        var instantTool = Instantiate(item.Prefab, _toolController.transform);
        if(instantTool.TryGetComponent<Tool>(out var tool)){
            _toolController.SetTool(tool);
        }
    }

    private void IncreaseIndex()
    {
        _currentIndex++;
        if(_currentIndex > _lastIndex)
        {
            _currentIndex = _firstIndex;
        }
        print(_inventory.GetSlot(_currentIndex).Item?.Id);
    }
    private void DecreaseIndex()
    {
        _currentIndex--;
        if (_currentIndex < _firstIndex)
        {
            _currentIndex = _lastIndex;
        }
        print(_inventory.GetSlot(_currentIndex).Item?.Id);
    }

    private void ChangeVisibility()
    {
        _isOpen = !_isOpen;

        if (!_isOpen)
        {
            _isSelectedSlot = false;
            _dragIcon.sprite = null;
            _dragAmount.text = string.Empty;
            _dragObject.SetActive(false);
        }
        _inventoryUI.SetActive(_isOpen);
    }
}

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
    private int _iconIndex;

    private PlayerController _playerController;
    private CameraHandler _cameraHandler;

    private bool _isOpen = false;

    private bool _isSelectedSlot;


    private void Start()
    {
        _inventory.InitializeSlots();
        _inventoryUI.Setup(_inventory);

        _playerController = ServiceLocator.Instance.GetService<PlayerController>();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();

        _playerController.OnClickEvent += OnClick;
        _playerController.OnInventoryClickEvent += ChangeVisibility;

        _inventoryUI.SetActive(_isOpen);
    }

    private void OnDestroy()
    {
        _playerController.OnClickEvent -= OnClick;
        _playerController.OnInventoryClickEvent -= ChangeVisibility;
    }

    private void Update()
    {
        if (_isSelectedSlot)
        {
            _dragObject.transform.position = Mouse.current.position.ReadValue();
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

    private void ChangeVisibility()
    {
        _isOpen = !_isOpen;

        _inventoryUI.SetActive(_isOpen);
    }
}

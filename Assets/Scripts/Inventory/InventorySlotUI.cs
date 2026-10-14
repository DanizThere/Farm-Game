using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlotUI : View, ISelectable, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int SlotIndex => _slotIndex;
    [SerializeField] private Image iconImage;
    [SerializeField] private CanvasGroup _canvasGroup;
    private RectTransform _imageRect;

    private Inventory _inventory;
    private int _slotIndex;

    private Color _originalColor;

    public virtual void Initialize(Inventory inventory, int index)
    {
        _inventory = inventory;
        _slotIndex = index;
        _originalColor = iconImage.color;
        _imageRect = iconImage.GetComponent<RectTransform>();

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        var slot = _inventory.GetSlot(_slotIndex);
        _imageRect.anchoredPosition = slot.Position;
        if (slot.IsEmpty)
        {
            iconImage.sprite = null;
            iconImage.color = _originalColor;
        }
        else
        {
            iconImage.sprite = slot.Item.Icon;
            iconImage.color = Color.white;
        }
    }

    public InventorySlot GetSlotByIndex()
    {
        return _inventory.GetSlot(_slotIndex);
    }

    public override void Show()
    {
        var slot = GetSlotByIndex();
        if (slot.IsEmpty) return;
    }

    public override void Hide()
    {
    }

    public bool CanSelect()
    {
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _imageRect.anchoredPosition += eventData.delta;
    }
}

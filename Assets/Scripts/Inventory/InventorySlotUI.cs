using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlotUI : View, ISelectable, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Inventory InventorySource => _inventory;
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

        var screenPoint = slot.Position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _imageRect.parent as RectTransform, screenPoint,
                null, out var local))
        {
            _imageRect.anchoredPosition = local;
        }

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

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _imageRect.parent as RectTransform, eventData.position,
            eventData.pressEventCamera, out var localPointer))
        {
            _imageRect.anchoredPosition = localPointer;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _imageRect.parent as RectTransform, eventData.position,
            eventData.pressEventCamera, out var localPointer))
        {
            _imageRect.anchoredPosition = localPointer;
        }
    }

    public Vector2 ScreenToLocal(RectTransform rect, Vector2 screenPoint, Camera cam)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rect, screenPoint, cam, out var local);
        return local;
    }
}

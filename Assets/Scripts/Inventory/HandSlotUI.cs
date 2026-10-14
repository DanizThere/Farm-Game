using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandSlotUI : View, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;
    private Inventory _inventory;
    private Canvas _canvas;
    private RectTransform _imageRect;

    private HandSlot _hand;

    private void Start()
    {
        _imageRect = _image.GetComponent<RectTransform>();
        _canvas = ServiceLocator.Instance.GetService<ViewController>().Canvas2D;
    }

    public override void Hide()
    {
        //here should to hide info about item
    }

    public override void Show()
    {
        //here should to show info about item
    }

    public void Initialize(Inventory inventory, HandSlot hand)
    {
        _inventory = inventory;
        _hand = hand;
    }

    public InventoryHandSlot GetSlot()
    {
        return _inventory.GetHandSlot(_hand.GetHand());
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsSlotNotExist()) return;
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsSlotNotExist()) return;
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsSlotNotExist()) return;
        _imageRect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            if(eventData.pointerDrag.TryGetComponent<InventorySlotUI>(out var ui))
            {
                var slot = ui.GetSlotByIndex();

                _inventory.AddItemToHandSlot(slot.Item, _hand.GetHand());
                _inventory.RemoveItem(ui.SlotIndex);

                _hand.SetItemObject(slot.Item.Prefab.GetComponent<ItemObject>());
            }
        }
    }

    public void UpdateSlot()
    {
        var slot = GetSlot();

        if (slot == null || slot.Item == null)
        {
            _image.sprite = null;
        }
        else
        {
            _image.sprite = slot.Item.Icon;
        }
    }

    private bool IsSlotNotExist()
    {
        var slot = GetSlot();
        if (slot == null || slot.Item == null) return true;
        return false;
    }
}

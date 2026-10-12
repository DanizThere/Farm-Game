using System;
using UnityEngine;
using UnityEngine.UI;

public class HandSlotUI : View
{
    [SerializeField] private Image _image;
    private Item _item;
    private HandSlot _slot;

    public override void Hide()
    {
    }

    public override void Show()
    {
    }

    public void Initialize(Item currentItem, HandSlot slot)
    {
        _item = currentItem ?? null;
        _slot = slot;

        _image.sprite = _item ? _item.Icon : null;
    }
}

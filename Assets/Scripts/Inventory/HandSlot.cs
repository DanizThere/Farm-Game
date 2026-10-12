using System;
using UnityEngine;

public class HandSlot : SLMonoBehaviour
{
    private HandController _hand;
    private InventoryController _inventory;
    private HandSlotUI _handSlotUI;
    private Item _currentItem;
    private HandController.Hand _handType;

    protected override void SLStart()
    {
        base.SLStart();
        _inventory = ServiceLocator.Instance.GetService<InventoryController>() ??
            throw new NullReferenceException($"{nameof(InventoryController)} doesn't exist");

        _inventory.AddSlot(this);
    }

    public Item GetItem() => _currentItem;

    public void Show()
    {
        var viewController = ServiceLocator.Instance.GetService<ViewController>();

        var prefab = Instantiate(viewController.Get<HandSlotUI>(), viewController.HandsSlotTransform);
        _handSlotUI = prefab;
        _handSlotUI.Initialize(_currentItem, this);

        if(_handType == HandController.Hand.Left)
        {
            _handSlotUI.transform.SetAsFirstSibling();
        }
        else
        {
            _handSlotUI.transform.SetAsLastSibling();
        }
    }

    public void Hide()
    {
        Destroy(_handSlotUI.gameObject);
        _handSlotUI = null;
    }

    public void Initialize(HandController hand, Item item = null)
    {
        _hand = hand;
        _handType = _hand.CurrentHandType;

        SetItem(item);
    }

    public void SetItem(Item item)
    {
        if (item == null) return;
        _currentItem = item;

        _inventory.Inventory.AddItem(item, Vector3.zero);
    }

    public void ClearItem()
    {
        _currentItem = null;
    }
}

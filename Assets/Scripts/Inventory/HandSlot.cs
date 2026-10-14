using System;

public class HandSlot : SLMonoBehaviour
{
    private HandController _hand;
    private InventoryController _inventory;
    private Item _currentItem;
    private HandController.Hand _handType;

    public string GetHand() => _handType.ToString();

    public void Initialize(HandController hand, InventoryController inventory, Item item = null)
    {
        _hand = hand;
        _handType = _hand.CurrentHandType;
        _inventory = inventory;

        _inventory.AddSlot(this);
        SetItem(item);
    }

    public void SetItem(Item item)
    {
        if (item == null)
        {
            _inventory.Inventory.RemoveItemFromHand(_handType.ToString());
            return;
        }
        _currentItem = item;
        _inventory.Inventory.AddItemToHandSlot(_currentItem, _handType.ToString());   
    }

    public void SetItemObject(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            return;
        }

        _hand.PickupItem(itemObject, fromWorld: false);
    }

    public void TryClearItem(string hand)
    {
        if(hand == _handType.ToString())
        {
            _currentItem = null;
            _hand.DeleteItem();
        }   
    }
}

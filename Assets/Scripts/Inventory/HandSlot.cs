using System;

public class HandSlot : SLMonoBehaviour
{
    public string InstanceId;
    private HandController _hand;
    private InventoryController _inventory;
    private Item _currentItem;
    private HandController.Hand _handType;

    public string GetHand() => _handType.ToString();

    public void Initialize(HandController hand, InventoryController inventory, Item item = null, string instanceId = null)
    {
        _hand = hand;
        _handType = _hand.CurrentHandType;
        _inventory = inventory;

        _inventory.AddSlot(this);
        SetItem(item, instanceId);
    }

    public void SetItem(Item item, string instanceId = null)
    {
        if (item == null)
        {
            print("Item is null");
            return;
        }
        InstanceId = instanceId;
        _currentItem = item;
        _inventory.Inventory.AddItemToHandSlot(_currentItem, _handType.ToString(), instanceId);   
    }

    public void ClearItem()
    {
        _currentItem = null;
        _inventory.Inventory.RemoveItemFromHand(_handType.ToString());
        _hand.DeleteItem();
    }

    public void SetItemObject(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            return;
        }

        _hand.PickupItem(itemObject, fromWorld: false);
    }
}

using UnityEngine;

public class ItemObjectHandler : MonoBehaviour, ISelectable
{
    [SerializeField] private ItemObject _itemObject;

    public bool CanSelect()
    {
        return true;
    }

    public ItemObject GetItemObject() => _itemObject;

    public void Hide()
    {
    }

    public void SetItemObject(ItemObject itemObject, ItemObjectParameters parameters = null)
    {
        _itemObject = Instantiate(itemObject);
        _itemObject.transform.SetParent(transform);
        _itemObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void Show()
    {
    }
}

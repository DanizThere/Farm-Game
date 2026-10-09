using UnityEngine;

public class ItemObjectHandler : MonoBehaviour
{
    [SerializeField] private ItemObject _itemObject;

    public ItemObject GetItemObject() => _itemObject;
    public void SetItemObject(ItemObject itemObject, ItemObjectParameters parameters = null)
    {
        _itemObject = Instantiate(itemObject);
        _itemObject.transform.SetParent(transform);
        _itemObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}

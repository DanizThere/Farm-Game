using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Chest : SLMonoBehaviour
{
    [SerializeField] private string _id = Guid.NewGuid().ToString();
    private Inventory _chestInventory;

    protected override void SLStart()
    {
        _chestInventory = GetComponent<Inventory>();
    }

    private void OnDestroy()
    {
        var data = _chestInventory.SaveData();
    }

    public InventorySlot GetSlot(int index)
    {
        return _chestInventory.GetSlot(index);
    }
}

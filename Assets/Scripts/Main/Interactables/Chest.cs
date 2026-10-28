using Assets.Scripts.Extenisions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Chest : SLMonoBehaviour, ISlappable
{
    public string Id => _id;
    [SerializeField] private string _id = Guid.NewGuid().ToString();
    private Inventory _chestInventory;
    private ChestService _chestService;
    private ViewController _viewController;

    private ChestUI _chestUI;
    private InventoryUI _inventoryUI;

    protected override void SLStart()
    {
        _chestInventory = GetComponent<Inventory>();
        _chestService = ServiceLocator.Instance.GetService<ChestService>();
        _viewController = ServiceLocator.Instance.GetService<ViewController>();
    }

    public InventorySlot GetSlot(int index)
    {
        return _chestInventory.GetSlot(index);
    }

    public bool AddItem(Item item, Vector2 position)
    {
        if(_chestInventory.AddItem(item, position))
        {
            _chestService.Put(GetSlots(), _id, transform.position, transform.rotation);

            return true;
        }

        return false;
    }

    private List<InventorySlot> GetSlots()
    {
        var slots = new List<InventorySlot>();
        for(int i = 0; i < _chestInventory.SlotsCount; i++)
        {
            var slot = GetSlot(i);
            slots.Add(slot);
        }

        return slots;
    }

    public void Slap()
    {
        var goChest = Instantiate(_viewController.Get<ChestUI>(), _viewController.Canvas2D.transform);
        var goInventory = Instantiate(_viewController.Get<InventoryUI>(), _viewController.Canvas2D.transform);

        _chestUI = goChest;
        _chestUI.Init(_chestInventory);
        _chestUI.OnClose += Close;

        _inventoryUI = goInventory;

        _viewController.SetCurrentView(_chestUI);

        _chestUI.Show();
        _inventoryUI.Show();

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
    }

    private void Close()
    {
        _chestService.Put(GetSlots(), _id, transform.position, transform.rotation);

        Destroy(_inventoryUI?.gameObject);

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
    }

    public void LoadItems(ChestSerialize info)
    {
        throw new NotImplementedException();
    }
}

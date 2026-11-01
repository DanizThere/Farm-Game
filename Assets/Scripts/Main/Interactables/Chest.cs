using Assets.Scripts.Extenisions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Chest : SLMonoBehaviour, ISlappable, ISaveableObject
{
    public Vector3Serialize Position => transform.position.ToSerializable();
    public QuaternionSerialize Rotation => transform.rotation.ToSerialize();
    public string InstanceId { get => _id; set => _id = value; }
    public string TypeTag => "Chest";
    public string ExtraData { get => JsonConvert.SerializeObject(GetSlotsSerializable()); }

    [SerializeField] private string _id;
    private Inventory _chestInventory;
    private ViewController _viewController;

    private ChestUI _chestUI;
    private InventoryUI _inventoryUI;

    private void Awake()
    {
        _chestInventory = GetComponent<Inventory>();
    }

    protected override void SLStart()
    {
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
            return true;
        }

        return false;
    }

    private List<InventoryItemSerialize> GetSlotsSerializable()
    {
        var slots = new List<InventoryItemSerialize>();
        for (int i = 0; i < _chestInventory.SlotsCount; i++)
        {
            var slot = GetSlot(i);
            slots.Add(new InventoryItemSerialize
            {
                Durability = slot.Durability,
                Id = slot.Item.Id,
                Position = slot.Position.ToSerializable(),
                Weight = slot.Item.Weight
            });
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

        ServiceLocator.Instance.GetService<InventoryController>().OpenChest(_chestUI, _inventoryUI);
    }

    private void Close()
    {
        if(_inventoryUI != null && _inventoryUI.gameObject != null)
            Destroy(_inventoryUI.gameObject);

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
    }

    public void Load(SaveObject data)
    {
        transform.SetPositionAndRotation(data.Position.ToVector(), data.Rotation.Deserialize());
        if (string.IsNullOrEmpty(data.ExtraData)) return;
        var items = JsonConvert.DeserializeObject<List<InventoryItemSerialize>>(data.ExtraData);
        var dbContext = ServiceLocator.Instance.GetService<GameDbContext>();
        foreach(var item in items)
        {
            var dbItem = dbContext.Items.Get(x => x.Id == item.Id).FirstOrDefault();
            if(dbItem != null)
            {
                AddItem(dbItem, item.Position.ToVector());
                continue;
            }

            print($"Item with id {item.Id} not found, skipped...");
        }
    }
}

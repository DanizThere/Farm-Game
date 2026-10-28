using System;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameDbBootstraper : Bootstraper
{
    private GameDbContext _context = new();
    [SerializeField] private DialogueNode[] _nodes;
    [SerializeField] private ItemObject[] _itemObjects;
    [SerializeField] private Item[] _items;
    [SerializeField] private PlantHealthCharacteristicData[] _characteristics;
    public override void Inject(ServiceLocator serviceLocator)
    {
        _context.DialogueNodes.AddRange(_nodes);
        _context.KeyItemObjectValue.AddRange(_itemObjects);
        _context.Items.AddRange(_items);
        _context.PlantHealthCharacteristics.AddRange(_characteristics);

        serviceLocator.Add(_context);
    }
}

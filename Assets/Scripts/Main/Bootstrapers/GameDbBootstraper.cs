using System;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameDbBootstraper : Bootstraper
{
    private GameDbContext _context = new();
    [SerializeField] private DialogueNode[] _nodes;
    [SerializeField] private ItemObjectHandler[] _handlers;
    [SerializeField] private Item[] _items;
    public override void Inject(ServiceLocator serviceLocator)
    {
        var keyItems = _handlers.Select(x => new KeyItemObjectValue
        {
            Handler = x
        });

        _context.DialogueNodes.AddRange(_nodes);
        _context.KeyItemObjectValue.AddRange(keyItems);
        _context.Items.AddRange(_items);

        serviceLocator.Add(_context);
    }
}

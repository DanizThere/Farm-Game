using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ItemSet<T> where T : class
{
    private readonly List<T> _items = new();

    public void Action(Action<T> act)
    {
        foreach (var item in _items)
        {
            act.Invoke(item);
        }
    }

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void AddRange(IEnumerable<T> items)
    {
        _items.AddRange(items);
    }

    public List<T> Get(Func<T, bool> func)
    {
        var result = _items.Where(func).ToList();
        return result;
    }

    public List<T> Get()
    {
        var result = _items.ToList();
        return result;
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }
}

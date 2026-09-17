using System;
using System.Collections.Generic;
using UnityEngine;

public class UIFactory : MonoBehaviour
{
    [SerializeField] private List<View> _prefabsList = new();
    private Dictionary<Type, View> _prefabs = new();

    public void Initialize()
    {
        foreach(var prefab in _prefabsList)
        {
            _prefabs.Add(prefab.GetType(), prefab);
        }
    }

    public T Create<T>() where T : View
    {
        if(!_prefabs.TryGetValue(typeof(T), out var prefab))
        {
            Debug.Log($"Requested {typeof(T)} not exist");

            return default;
        }

        var instantiate = Instantiate(prefab);
        return (T)instantiate;
    }
}

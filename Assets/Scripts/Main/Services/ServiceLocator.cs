using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerSettings _playerSettings;

    private Dictionary<Type, IService> _services = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(Instance);

        ISaveService saveService = new JsonSaveService();

        Add(saveService);
        Add(_playerController);

        _playerController.Setup(_playerSettings);
    }

    public void Add(IService service)
    {
        if (_services.ContainsKey(service.GetType())) 
        { 
            return;
        }

        _services.Add(service.GetType(), service);
    }

    public T GetService<T>()
    {
        _services.TryGetValue(typeof(T), out var service);

        return (T)service;
    }
}

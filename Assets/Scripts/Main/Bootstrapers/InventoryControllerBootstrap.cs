using System;
using UnityEngine;

[Serializable]
public class InventoryControllerBootstrap : Bootstraper
{
    [SerializeField] private InventoryController _controller;

    public override void Inject(ServiceLocator serviceLocator)
    {
        var go = GameObject.Instantiate(_controller, serviceLocator.transform);
        serviceLocator.Add(go);
    }
}

using System;
using UnityEngine;

[Serializable]
public class ChestServiceBootstraper : Bootstraper
{
    private ChestService _chestService;

    public override void Inject(ServiceLocator serviceLocator)
    {
        _chestService = new();
        serviceLocator.Add(_chestService);
    }
}

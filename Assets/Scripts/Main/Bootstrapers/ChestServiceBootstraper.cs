using System;
using UnityEngine;

[Serializable]
public class ChestServiceBootstraper : Bootstraper
{
    [SerializeField] private Chest _prefab;
    private ChestService _chestService;

    public override void Inject(ServiceLocator serviceLocator)
    {
        _chestService = new();
        _chestService._basePrefab = _prefab;
        serviceLocator.Add(_chestService);
    }
}

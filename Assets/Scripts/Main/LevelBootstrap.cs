using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private ItemObjectController _toolController;
    [SerializeField] private ViewController _viewController;
    [SerializeField] private ItemDatabase _itemDatabase;

    [SerializeReference] private List<Bootstraper> _bootstrapers = new();

    [SerializeField] private LevelEntryPoint _levelEntryPoint;

    private void Awake()
    {
        Add();
    }

    private void Add()
    {
        foreach(var boot in _bootstrapers)
        {
            boot.Inject(ServiceLocator.Instance);
        }

        ServiceLocator.Instance.Add(_toolController);
        ServiceLocator.Instance.Add(_viewController);
        ServiceLocator.Instance.Add(_itemDatabase);

        Construct();
    }

    private void Construct()
    {
        var saveService = ServiceLocator.Instance.GetService<JsonSaveService>();
        StartCoroutine(_levelEntryPoint.Initialize(saveService));
    }
}

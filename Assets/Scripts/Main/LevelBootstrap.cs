using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeReference] private List<Bootstraper> _bootstrapers = new();

    [SerializeField] private AbstractEntryPoint _levelEntryPoint;

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

        Construct();
    }

    private void Construct()
    {
        StartCoroutine(_levelEntryPoint.Initialize());
    }
}

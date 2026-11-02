using System;
using UnityEngine;

[Serializable]
public class SFXManagerBootstraper : Bootstraper
{
    [SerializeField] private SFXManager _manager;
    public override void Inject(ServiceLocator serviceLocator)
    {
        var go = GameObject.Instantiate(_manager, serviceLocator.transform);
        serviceLocator.Add(go);
    }
}

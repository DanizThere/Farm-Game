using System;
using UnityEngine;

[Serializable]
public class MusicManagerBootstraper : Bootstraper
{
    [SerializeField] private MusicManager _manager;
    public override void Inject(ServiceLocator serviceLocator)
    {
        var go = GameObject.Instantiate(_manager, serviceLocator.transform);
        serviceLocator.Add(go);
    }
}

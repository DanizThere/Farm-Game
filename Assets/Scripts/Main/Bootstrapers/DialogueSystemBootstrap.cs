using System;
using UnityEngine;

[Serializable]
public class DialogueSystemBootstrap : Bootstraper
{
    [SerializeField] private DialogueSystem _prefab;
    public override void Inject(ServiceLocator serviceLocator)
    {
        var go = GameObject.Instantiate(_prefab, serviceLocator.transform);
        serviceLocator.Add(go);
    }
}

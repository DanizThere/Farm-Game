using System;
using UnityEngine;

[Serializable]
public class ViewControllerBootstraper : Bootstraper
{
    [SerializeField] private View[] _viewsPrefabs;
    [SerializeField] private ViewController _viewController;
    public override void Inject(ServiceLocator serviceLocator)
    {
        var go = GameObject.Instantiate(_viewController, serviceLocator.transform);
        foreach(var view in _viewsPrefabs)
        {
            go.Add(view);
        }

        serviceLocator.Add(go);
    }
}

using System.Collections;
using UnityEngine;

[System.Serializable]
public class CameraHandlerBootstrap : Bootstraper
{
    [SerializeField] private CameraHandler _cameraHandler;
    public override void Inject(ServiceLocator serviceLocator)
    {
        var go = GameObject.Instantiate(_cameraHandler, serviceLocator.transform);
        serviceLocator.Add(go);
    }
}
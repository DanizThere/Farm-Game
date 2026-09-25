using System.Collections;
using UnityEngine;

[System.Serializable]
public class CameraHandlerBootstrap : Bootstraper
{
    [SerializeField] private CameraHandler _cameraHandler;
    public override void Inject(ServiceLocator serviceLocator)
    {
        serviceLocator.Add(_cameraHandler);
    }
}
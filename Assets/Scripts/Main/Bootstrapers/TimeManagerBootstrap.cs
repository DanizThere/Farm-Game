using System.Collections;
using UnityEngine;

[System.Serializable]
public class TimeManagerBootstrap : Bootstraper
{
    [SerializeField] private TimeManager _timeManager;
    public override void Inject(ServiceLocator serviceLocator)
    {
        serviceLocator.Add(_timeManager);
    }
}
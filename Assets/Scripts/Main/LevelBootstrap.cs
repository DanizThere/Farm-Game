using System;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private ToolController _toolController;

    private void Awake()
    {
        Add();

        Construct();
    }

    private void Add()
    {
        ServiceLocator.Instance.Add(_timeManager);
        ServiceLocator.Instance.Add(_cameraHandler);
        ServiceLocator.Instance.Add(_toolController);
    }

    private void Construct()
    {
        _timeManager.Setup();
    }
}

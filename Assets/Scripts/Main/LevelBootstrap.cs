using System;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private ItemObjectController _toolController;
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private ViewController _viewController;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private DialogueSystem _dialogueSystem;

    [SerializeField] private LevelEntryPoint _levelEntryPoint;

    private void Awake()
    {
        Add();

        Construct();

    }

    private void Start()
    {
    }

    private void Add()
    {
        ServiceLocator.Instance.Add(_timeManager);
        ServiceLocator.Instance.Add(_cameraHandler);
        ServiceLocator.Instance.Add(_cameraHandler);
        ServiceLocator.Instance.Add(_toolController);
        ServiceLocator.Instance.Add(_gameStateMachine);
        ServiceLocator.Instance.Add(_viewController);
        ServiceLocator.Instance.Add(_itemDatabase);
        ServiceLocator.Instance.Add(_dialogueSystem);
    }

    private void Construct()
    {
        var saveService = ServiceLocator.Instance.GetService<JsonSaveService>();
        StartCoroutine(_levelEntryPoint.Initialize(saveService));
    }
}

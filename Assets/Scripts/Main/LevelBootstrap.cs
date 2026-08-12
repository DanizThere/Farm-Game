using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private ToolController _toolController;
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private ViewController _viewController;

    private void Awake()
    {
        Add();

        Construct();
    }

    private void Add()
    {
        ServiceLocator.Instance.Add(_timeManager);
        ServiceLocator.Instance.Add(_cameraHandler);
        ServiceLocator.Instance.Add(_cameraHandler);
        ServiceLocator.Instance.Add(_toolController);
        ServiceLocator.Instance.Add(_gameStateMachine);
        ServiceLocator.Instance.Add(_viewController);
    }

    private void Construct()
    {
        _timeManager.Setup();
    }
}

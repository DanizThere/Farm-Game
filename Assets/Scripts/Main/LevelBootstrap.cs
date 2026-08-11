using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private CameraHandler _cameraHandler;

    private void Awake()
    {
        Add();

        Construct();
    }

    private void Add()
    {
        ServiceLocator.Instance.Add(_timeManager);
        ServiceLocator.Instance.Add(_cameraHandler);
    }

    private void Construct()
    {
        _timeManager.Setup();
    }
}

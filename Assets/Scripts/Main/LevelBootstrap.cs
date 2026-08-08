using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;

    private void Awake()
    {
        Add();

        Construct();
    }

    private void Add()
    {
        ServiceLocator.Instance.Add(_timeManager);
    }

    private void Construct()
    {
        _timeManager.Setup();
    }
}

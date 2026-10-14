using System;
using System.Collections;
using UnityEngine;

public class LevelEntryPoint : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private DirtController _dirtController;

    [SerializeField] private PlayerInitializer _playerInitializer;

    public IEnumerator Initialize()
    {
        _dirtController.Generate();

        _timeManager.Setup();
        ServiceLocator.Instance.GetService<GameStateMachine>().InitializeStates();

        _playerInitializer.Initialize();

        ServiceLocator.Instance.GetService<ViewController>().Initialize();
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();

        yield return null;
    }
}

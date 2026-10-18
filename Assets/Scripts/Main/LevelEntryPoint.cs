using System;
using System.Collections;
using UnityEngine;

public class LevelEntryPoint : AbstractEntryPoint
{
    [SerializeField] private DirtController _dirtController;

    [SerializeField] private PlayerInitializer _playerInitializer;
    [SerializeField] private SaveHandler _saveHandler;

    public override IEnumerator Initialize()
    {
        _dirtController.Generate();
        _saveHandler.Initialize();

        ServiceLocator.Instance.GetService<GameStateMachine>().InitializeStates();

        _playerInitializer.Initialize();

        ServiceLocator.Instance.GetService<ViewController>().Initialize();
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();

        yield return null;
    }
}

using System;
using System.Collections;
using UnityEngine;

public class LevelEntryPoint : AbstractEntryPoint
{
    [SerializeField] private DirtController _dirtController;

    [SerializeField] private PlayerInitializer _playerInitializer;
    [SerializeField] private SaveHandler _saveHandler;
    [SerializeField] private bool _toggleToDependingMusic = true;

    public override IEnumerator Initialize()
    {
        ServiceLocator.Instance.GetService<GameStateMachine>().InitializeStates();
        if(_toggleToDependingMusic) ServiceLocator.Instance.GetService<MusicManager>().ToggleMusic();

        _dirtController.Generate();
        _saveHandler.Initialize(_playerInitializer);
        ServiceLocator.Instance.GetService<ViewController>().Initialize();

        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();

        yield return null;
    }
}

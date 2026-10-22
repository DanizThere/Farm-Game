using System.Collections;
using UnityEngine;

public class TutorialEntryPoint : AbstractEntryPoint
{
    [SerializeField] private PlayerInitializer _playerInitializer;
    [SerializeField] private PlayerData _playerData;

    public override IEnumerator Initialize()
    {
        ServiceLocator.Instance.GetService<GameStateMachine>().InitializeStates();
        ServiceLocator.Instance.GetService<TimeManager>().Setup();

        _playerInitializer.Initialize(_playerData);

        ServiceLocator.Instance.GetService<ViewController>().Initialize();
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();

        yield return null;
    }
}

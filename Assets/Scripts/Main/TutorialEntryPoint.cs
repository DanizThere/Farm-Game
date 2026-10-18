using System.Collections;
using UnityEngine;

public class TutorialEntryPoint : AbstractEntryPoint
{
    [SerializeField] private PlayerInitializer _playerInitializer;

    public override IEnumerator Initialize()
    {
        ServiceLocator.Instance.GetService<GameStateMachine>().InitializeStates();
        ServiceLocator.Instance.GetService<TimeManager>().Setup();

        _playerInitializer.Initialize();

        ServiceLocator.Instance.GetService<ViewController>().Initialize();
        ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();

        yield return null;
    }
}

using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameStateBootstraper : Bootstraper
{
    [SerializeField] private GameStateMachine _gameState;

    public override void Inject(ServiceLocator serviceLocator)
    {
        serviceLocator.Add(_gameState);
    }
}

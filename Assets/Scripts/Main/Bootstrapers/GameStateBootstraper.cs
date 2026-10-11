using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameStateBootstraper : Bootstraper
{
    [SerializeField] private GameStateMachine _gameState;

    public override void Inject(ServiceLocator serviceLocator)
    {
        var go = GameObject.Instantiate(_gameState, serviceLocator.transform);
        serviceLocator.Add(go);
    }
}

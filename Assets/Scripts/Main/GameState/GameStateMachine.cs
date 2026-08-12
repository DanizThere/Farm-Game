using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour, IService
{
    [SerializeField] private List<GameState> _gameStates = new();

    private Dictionary<Type, GameState> _gameStatesDictionary = new();

    private GameState _currentState;

    private void Start()
    {
        InitializeStates();
    }

    private void Update()
    {
        _currentState?.OnUpdate();
    }

    public void InitializeStates()
    {
        foreach(var state in _gameStates)
        {
            if (_gameStatesDictionary.TryGetValue(state.GetType(), out var _)) continue;

            _gameStatesDictionary.Add(state.GetType(), state);
        }
    }

    public T GetState<T>() where T : GameState
    {
        _gameStatesDictionary.TryGetValue(typeof(T), out var state);
        return (T)state;
    }

    public void SetState<T>() where T : GameState
    {
        if(_gameStatesDictionary.TryGetValue(typeof(T), out var state))
        {
            OnUpdateState(state);

            return;
        }

        Debug.LogWarning($"{typeof(T)} doesn't exist", gameObject);
    }

    private void OnUpdateState(GameState state)
    {
        _currentState?.OnExit();

        _currentState = state;
        _currentState.OnEnter();
    }
}

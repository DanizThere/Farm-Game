using System;
using System.Collections;
using UnityEngine;

public class LevelEntryPoint : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private NewGameInventory _newGameInventory;
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private DirtController _dirtController;

    [SerializeField] private DialogueHandler _startNewGameDialogue;
    [SerializeField] private DialogueSystem _dialogueSystem;

    [SerializeField] private PlayerInitializer _playerInitializer;

    public IEnumerator Initialize(JsonSaveService saveService)
    {
        _dirtController.Generate();

        _timeManager.Setup();
        _gameStateMachine.InitializeStates();

        _playerInitializer.Initialize();

        if (!saveService.FileExists<InventorySerializable>("InventoryData"))
        {
            ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
            _newGameInventory.Initialize();
            _startNewGameDialogue.StartDialogue(_dialogueSystem);

            yield break;
        }

        //ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();

        yield return null;
    }
}

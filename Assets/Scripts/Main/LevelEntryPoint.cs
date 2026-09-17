using System;
using System.Collections;
using UnityEngine;

public class LevelEntryPoint : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private NewGameInventory _newGameInventory;
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private DirtController _dirtController;

    [SerializeField] private DialogueHandler _startNewGameDialogue;
    [SerializeField] private DialogueSystem _dialogueSystem;

    public IEnumerator Initialize(JsonSaveService saveService)
    {
        _dirtController.Generate();

        _timeManager.Setup();
        _gameStateMachine.InitializeStates();

        _inventoryController.Setup();

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

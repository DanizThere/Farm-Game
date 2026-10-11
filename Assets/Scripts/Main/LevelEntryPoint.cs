using System;
using System.Collections;
using UnityEngine;

public class LevelEntryPoint : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private NewGameInventory _newGameInventory;
    [SerializeField] private DirtController _dirtController;

    [SerializeField] private DialogueHandler _startNewGameDialogue;
    [SerializeField] private DialogueSystem _dialogueSystem;

    [SerializeField] private PlayerInitializer _playerInitializer;

    public IEnumerator Initialize()
    {
        _dirtController.Generate();

        _timeManager.Setup();
        ServiceLocator.Instance.GetService<GameStateMachine>().InitializeStates();

        _playerInitializer.Initialize();

        ServiceLocator.Instance.GetService<ViewController>().Initialize();

        var saveService = ServiceLocator.Instance.GetService<JsonSaveService>();
        if (!saveService.FileExists<InventorySerializable>("InventoryData"))
        {
            ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
            _newGameInventory.Initialize();
            _startNewGameDialogue.StartDialogue(_dialogueSystem);

            yield break;
        }
        else
        {
            ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>();
        }

        yield return null;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    private CameraHandler _cameraHandler;

    public void Initialize()
    {
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnUpdateEvent += SecondDimensionInteract;
    }

    private void SecondDimensionInteract()
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
    }
}

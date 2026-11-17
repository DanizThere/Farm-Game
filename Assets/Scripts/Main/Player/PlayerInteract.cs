using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    private CameraHandler _cameraHandler;
    private float _interactDistance;
    private OutlineInteractor _interactor;

    public void Initialize(PlayerData data)
    {
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();

        _interactDistance = data.HandDistance;
        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnUpdateEvent += SecondDimensionInteract;
        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>().OnUpdateEvent += ThirdDimensionInteract;
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

    private void ThirdDimensionInteract()
    {
        if(Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit, _interactDistance))
        {
            if(hit.transform.TryGetComponent<OutlineInteractor>(out var interactor))
            {
                if (interactor == _interactor) return;
                _interactor = interactor;
                _interactor.Show();
                
                return;
            }
            _interactor?.Hide();
            _interactor = null;
            return;
        }

        _interactor?.Hide();
        _interactor = null;
    }
}

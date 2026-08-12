using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private Observable<ISelectable> _selectableObservable;
    private ISelectable _selectable;
    private CameraHandler _cameraHandler;

    private void Start()
    {
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>().OnUpdateEvent += ThirdDimensionInteract;
        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnUpdateEvent += SecondDimensionInteract;

        _selectableObservable = new(_selectable);
        _selectableObservable.ValueChanged += view => view?.Show();
    }

    private void SecondDimensionInteract()
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<ISelectable>(out _selectable))
            {
                _selectableObservable.Value = _selectable;
                break;
            }
            else
            {
                Hide();
            }
        }
    }

    private void ThirdDimensionInteract()
    {
        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit))
        {
            if (hit.transform.TryGetComponent<ISelectable>(out _selectable))
            {
                _selectableObservable.Value = _selectable;
                return;
            }
            else
            {
                Hide();
                return;
            }
        }
        else
        {
            Hide();
            return;
        }
    }

    private void Hide()
    {
        _selectableObservable?.Value?.Hide();
        _selectable = null;
        _selectableObservable.Value = _selectable;
    }
}

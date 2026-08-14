using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private Observable<ISelectable> _selectableObservable;
    private ISelectable _selectable;
    private Transform _selectableTransform;
    private CameraHandler _cameraHandler;
    private PlayerController _playerController;

    private void Start()
    {
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();

        _playerController.OnInteractEvent += TryInteract;

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
                _selectableTransform = result.gameObject.transform;
                break;
            }
            //else
            //{
            //    if(_selectable != null)
            //    Hide();
            //}
        }
    }

    private void ThirdDimensionInteract()
    {
        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit))
        {
            if (hit.transform.TryGetComponent<ISelectable>(out _selectable))
            {
                _selectableObservable.Value = _selectable;
                _selectableTransform = hit.transform;
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

    private void TryInteract()
    {
        if (_selectableObservable.Value == null) return;
        
        if(_selectableTransform.TryGetComponent<ISelectableActions>(out var actions))
        {
            actions.ShowActions();
            actions.SetTarget(transform);
            ServiceLocator.Instance.GetService<GameStateMachine>().SetState<UIGameState>();
        }
    }

    private void Hide()
    {
        _selectableObservable?.Value?.Hide();
        _selectableTransform = null;
        _selectable = null;
        _selectableObservable.Value = _selectable;
    }
}

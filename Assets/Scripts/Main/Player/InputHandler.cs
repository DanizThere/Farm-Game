using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IService
{
    public Vector3 Direction => _direction; 
    public Vector3 LookDirection => _lookDirection; 

    public event Action OnSprintEvent = delegate { };
    public event Action OnSprintHoldedEvent = delegate { };
    public event Action OnSprintReleasedEvent = delegate { };

    public event Action<int> OnDigitClickEvent = delegate { };

    public event Action OnThrowEvent = delegate { };

    public event Action OnEscapeEvent = delegate { };

    public event Action OnNextClickEvent = delegate { };
    public event Action OnBackClickEvent = delegate { };

    public event Action OnInteractEvent = delegate { };

    public event Action OnInventoryClickEvent = delegate { };

    public event Action OnJumpEvent = delegate { };

    public event Action OnClickEvent = delegate { };
    public event Action OnClickHoldedEvent = delegate { };
    public event Action OnClickReleased = delegate { };

    public event Action OnRightClickEvent = delegate { };
    public event Action OnRightClickHoldedEvent = delegate { };
    public event Action OnRightClickReleased = delegate { };

    public event Action OnRunClickEvent = delegate { };
    public event Action OnRunClickReleasedEvent = delegate { };

    private PlayerSettings _playerSettings;

    private Vector3 _direction;
    private Vector3 _lookDirection;

    private Vector3 _rawDirection;
    private Vector3 _rawLookDirection;

    private float _clickPressTime, _clickRightPressTime, _sprintPressTime;

    private void Update()
    {
        _direction = Vector3.Lerp(_direction, _rawDirection, Time.deltaTime * _playerSettings.Speed);
        _lookDirection = Vector3.Lerp(_lookDirection, _rawLookDirection, Time.deltaTime * _playerSettings.LookSensevity);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnRunClickEvent?.Invoke();
        }
        if (context.canceled)
        {
            OnRunClickReleasedEvent?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _sprintPressTime = Time.time;
            OnSprintEvent?.Invoke();
        }
        if (context.canceled)
        {
            var holdDuration = Time.time - _sprintPressTime;
            if (holdDuration < .2f)
            {
                OnSprintReleasedEvent?.Invoke();
            }
            OnSprintHoldedEvent?.Invoke();
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnThrowEvent?.Invoke();
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnEscapeEvent?.Invoke();
        }
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnNextClickEvent?.Invoke();
        }
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnBackClickEvent?.Invoke();
        }
    }

    public void OnDigit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var number = (int)context.ReadValue<float>();

            OnDigitClickEvent?.Invoke(number);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteractEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _rawDirection = new Vector3(direction.x, 0, direction.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _rawLookDirection = new Vector3(direction.y, 0, direction.x);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _clickPressTime = Time.time;
            OnClickEvent?.Invoke();
        }
        if (context.canceled)
        {
            var holdDuration = Time.time - _clickPressTime;
            if(holdDuration < .2f)
            {
                OnClickReleased?.Invoke();
            }
            OnClickHoldedEvent?.Invoke();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _clickRightPressTime = Time.time;
            OnRightClickEvent?.Invoke();
        }
        if (context.canceled)
        {
            var holdDuration = Time.time - _clickRightPressTime;
            if (holdDuration < .2f)
            {
                OnRightClickReleased?.Invoke();
            }
            OnRightClickHoldedEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnJumpEvent?.Invoke();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInventoryClickEvent?.Invoke();
        }
    }

    public void Setup(PlayerSettings playerSettings)
    {
        _playerSettings = playerSettings;
    }
}

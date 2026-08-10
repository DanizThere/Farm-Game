using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IService
{
    public Vector3 Direction => _direction; 
    public Vector3 LookDirection => _lookDirection; 

    public event Action OnJumpEvent = delegate { };
    public event Action OnClickEvent = delegate { };
    public event Action<float> OnClickHoldedEvent = delegate { };
    public event Action OnClickReleased = delegate { };

    private PlayerSettings _playerSettings;

    private Vector3 _direction;
    private Vector3 _lookDirection;

    private Vector3 _rawDirection;
    private Vector3 _rawLookDirection;

    private float _clickPressTime;

    private void Update()
    {
        _direction = Vector3.Lerp(_direction, _rawDirection, Time.deltaTime * _playerSettings.Speed);
        _lookDirection = Vector3.Lerp(_lookDirection, _rawLookDirection, Time.deltaTime * _playerSettings.LookSensevity);
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
            OnClickHoldedEvent?.Invoke(holdDuration);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnJumpEvent?.Invoke();
        }
    }

    public void Setup(PlayerSettings playerSettings)
    {
        _playerSettings = playerSettings;
    }
}

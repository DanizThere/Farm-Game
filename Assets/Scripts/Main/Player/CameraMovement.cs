using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IService
{
    public Camera Camera => _camera;

    public float XRotation => _xRotation;
    public float YRotation => _yRotation;

    public bool IsXNearBottom(float value) => _xRotation > _topBorder - value;
    public bool IsXNearTop(float value) => _xRotation < _bottomBorder + value;

    [SerializeField] private float _bottomBorder, _topBorder;
    [SerializeField] private float _mouseSensevity = 100f;
    [SerializeField] private float _smooth = 15f;
    [SerializeField] private Transform _body;

    private bool _canMove = true;

    private float _currentFOV;
    private float _defaultFOV;

    private float _yRotation, _xRotation;
    private float _leftBorder, _rightBorder;

    private Camera _camera;

    private InputHandler _playerController;
    private Vector2 _previousPosition;


    public void Initialize(Transform body)
    {
        _body = body;
        _playerController = ServiceLocator.Instance.GetService<InputHandler>();

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>().OnExitEvent += () => SetMove(false, () =>
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        });
        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>().OnStartEvent += () => SetMove(true, () =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        });

        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _defaultFOV = _camera.fieldOfView;
        _currentFOV = _camera.fieldOfView;
    }

    private void Update()
    {
        Move(_playerController.LookDirection);
    }

    public void SetForwardLook()
    {
        var lookAt = Quaternion.Euler(transform.forward);
        transform.rotation = lookAt;
        _body.rotation = lookAt;
    }

    public void IncrementFOV(float fov = 1)
    {
        _currentFOV = Mathf.Clamp(_currentFOV + fov, _defaultFOV, 80);
        _camera.fieldOfView = _currentFOV;
    }

    public void DecrementFOV(float fov = 1)
    {
        _currentFOV = Mathf.Clamp(_currentFOV - fov, _defaultFOV, 80);
        _camera.fieldOfView = _currentFOV;
    }

    public void ResetFOV()
    {
        _currentFOV = _defaultFOV;
        _camera.fieldOfView = _currentFOV;
    }

    public void SetXClamp(float clamp)
    {
        _leftBorder = -clamp;
        _rightBorder = clamp;
    }

    public void ResetXClamp()
    {
        _leftBorder = 0f;
        _rightBorder = 0f;
    }

    public void SetMove(bool move, Action callback = null)
    {
        _canMove = move;

        callback?.Invoke();
    }

    public void Move(Vector3 mousePosition)
    {
        var newPosition = new Vector2(mousePosition.z, mousePosition.y);
        //if ((newPosition - _previousPosition).magnitude == 0f) return;

        var mouseX = mousePosition.z * Time.deltaTime * _mouseSensevity;
        var mouseY = mousePosition.x * Time.deltaTime * _mouseSensevity;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        if (_leftBorder > 0) _yRotation = Mathf.Clamp(_yRotation, _leftBorder, _rightBorder);
        _xRotation = Mathf.Clamp(_xRotation, _bottomBorder, _topBorder);

        if (!_canMove) return;
        var rotation = Quaternion.Euler(0, _yRotation, 0);

        var newRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _smooth);
        _previousPosition = newPosition;
        _body.rotation = rotation;
    }
}

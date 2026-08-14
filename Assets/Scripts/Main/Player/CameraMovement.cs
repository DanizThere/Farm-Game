using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IService
{
    public Camera Camera => _camera;

    [SerializeField] private float _bottomBorder, _topBorder;
    [SerializeField] private float _mouseSensevity = 100f;
    [SerializeField] private Transform _body;

    private bool _canMove = true;

    private float _currentFOV;
    private float _defaultFOV;

    private float _yRotation, _xRotation;
    private float _leftBorder, _rightBorder;

    private Camera _camera;

    private PlayerController _playerController;


    private void Start()
    {
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnStartEvent += () => SetMove(false, () =>
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        });
        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnExitEvent += () => SetMove(true, () =>
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
        if (!_canMove) return;

        var mouseX = mousePosition.z * Time.deltaTime * _mouseSensevity;
        var mouseY = mousePosition.x * Time.deltaTime * _mouseSensevity;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        if (_leftBorder > 0) _yRotation = Mathf.Clamp(_yRotation, _leftBorder, _rightBorder);
        _xRotation = Mathf.Clamp(_xRotation, _bottomBorder, _topBorder);

        var rotation = Quaternion.Euler(0, _yRotation, 0);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _body.rotation = rotation;
    }
}

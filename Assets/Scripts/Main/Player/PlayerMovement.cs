using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IFreezeable, IMovement
{
    public Transform Body => _body;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _body;

    [SerializeField] private float _localSpeed = 1f;
    [SerializeField] private float _runningSpeed = 3f;

    [SerializeField] private float _gravityForce = 9.81f;
    [SerializeField] private float _jumpForce = 1.5f;
    private Vector3 _velocity;

    private float _currentSpeed;
    private float _speed;
    private bool _isRunning;

    private InputHandler _controller;
    
    private float _timeScale = 1f;


    public void Initialize()
    {
        _controller = ServiceLocator.Instance.GetService<InputHandler>();
        _speed = _localSpeed;

        _controller.OnJumpEvent += Jump;
        _controller.OnSprintEvent += OnRun;
        _controller.OnSprintReleasedEvent += OnWalk;
        _controller.OnSprintHoldedEvent += OnWalk;
    }

    private void Update()
    {
        if(_controller)
            Move(_controller.Direction);

        _speed = Mathf.Lerp(_speed, _currentSpeed, Time.deltaTime * 10f);
        print(_currentSpeed);
        _currentSpeed = _isRunning ? _runningSpeed : _localSpeed;
    }

    private void FixedUpdate()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravityForce * Time.deltaTime * -1 * _timeScale;

        _characterController.Move(_velocity);
    }

    private void OnDestroy()
    {
        _controller.OnJumpEvent -= Jump;
        _controller.OnSprintHoldedEvent -= OnRun;
        _controller.OnSprintReleasedEvent -= OnWalk;
    }

    private void OnRun()
    {
        _isRunning = true;
    }
    private void OnWalk()
    {
        _isRunning = false;
    }

    public void SetTimeScale(float timeScale)
    {
        _timeScale = timeScale;
    }

    public void Move(Vector3 move)
    {
        var direction = _body.forward * move.z + _body.right * move.x;
        _characterController.Move(_currentSpeed * Time.deltaTime * direction);
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * -_gravityForce * _timeScale);
        }
    }
}

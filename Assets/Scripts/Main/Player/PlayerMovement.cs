using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IFreezeable, IMovement
{
    public Transform Body => _body;
    public Transform Model => _model;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _model;
    [SerializeField] private Animator _modelAnimator;

    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _footstepFrequency = 1f;
    [SerializeField] private AudioClip _footstepSound;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _landSound;

    [Space]
    [SerializeField] private float _gravityForce = 9.81f;
    [SerializeField] private float _jumpForce = 1.5f;
    private Vector3 _velocity;

    private float _moveSpeed;
    private float _runSpeed;

    private float _currentSpeed;
    private float _speed;
    private bool _isRunning;

    private InputHandler _controller;
    
    private float _timeScale = 1f;
    private float _speedScale = 1f;

    private List<HandController> _hands = new();
    private bool _moveBool = false;
    private float _footstepTimer = 0f;
    private bool _wasGrounded;


    public void Initialize(List<HandController> handControllers, PlayerData data)
    {
        _hands = handControllers;
        _controller = ServiceLocator.Instance.GetService<InputHandler>();

        _runSpeed = data.RunSpeed;
        _moveSpeed = data.MoveSpeed;
        _jumpForce = data.JumpForce;

        _speed = _currentSpeed = _moveSpeed;

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>().OnStartEvent += () =>
        {
            _controller.OnJumpEvent += Jump;
            _controller.OnSprintEvent += OnRun;
            _controller.OnSprintReleasedEvent += OnWalk;
            _controller.OnSprintHoldedEvent += OnWalk;
        };

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<OuterWorldGameState>().OnExitEvent += () =>
        {
            _controller.OnJumpEvent -= Jump;
            _controller.OnSprintEvent -= OnRun;
            _controller.OnSprintReleasedEvent -= OnWalk;
            _controller.OnSprintHoldedEvent -= OnWalk;
        };
    }

    private void Update()
    {
        if (_controller)
            Move(_controller.Direction);

        _speed = Mathf.Lerp(_speed, _currentSpeed, Time.deltaTime * 10f);
        _currentSpeed = _isRunning ? _runSpeed : _moveSpeed;
        PlayFootstepSound();

        var isGrounded = Physics.Raycast(transform.position, Vector3.down, _characterController.height * .5f + .1f);
        if (isGrounded && !_wasGrounded)
        {
            _audioSource.PlayOneShot(_landSound);
        }

        _wasGrounded = isGrounded;
    }

    private void PlayFootstepSound()
    {
        if (_footstepTimer >= _footstepFrequency)
        {
            _audioSource.PlayOneShot(_footstepSound);
            _footstepTimer = 0f;
            return;
        }

        var isGrounded = Physics.Raycast(transform.position, Vector3.down, _characterController.height * .5f + .1f);
        if (isGrounded && _characterController.velocity.magnitude > .1f)
        {
            _footstepTimer += Time.deltaTime;
            return;
        }

        _footstepTimer = 0f;
    }

    private void FixedUpdate()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravityForce * -1 * _timeScale * Time.fixedDeltaTime;

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
        var anyHold = _hands.Where(x => x.IsTotalBusy).ToList();

        var direction = _body.forward * move.z + _body.right * move.x;
        var delta = _currentSpeed * Time.deltaTime * direction * _speedScale;
        var newPosition = _body.position + delta;

        _moveBool = (newPosition - _body.position).magnitude > 0f;
        _modelAnimator.SetBool("Move", _moveBool);


        if (anyHold.Any())
        {
            var hand = anyHold[0];
            var offset = newPosition - hand.transform.position;

            if (offset.sqrMagnitude > hand.HandDistance * hand.HandDistance)
            {
                newPosition = hand.transform.position + offset.normalized * hand.HandDistance;
            }
            //else
            //{
            //    newPosition = Vector3.Lerp(newPosition, hand.transform.position, Time.deltaTime * _timeScale * .5f);
            //}
        }

        _characterController.Move(newPosition - _body.position);
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -1f * -_gravityForce * _timeScale);
            _modelAnimator.SetTrigger("Jump");
            _audioSource.PlayOneShot(_jumpSound);
        }
    }

    public void SetSpeedScale(float obj)
    {
        _speedScale = obj;
    }
}

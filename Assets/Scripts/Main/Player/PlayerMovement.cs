using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IFreezeable, IMovement
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _body;

    [SerializeField] private float _localSpeed = 1f;

    [SerializeField] private float _gravityForce = 9.81f;
    [SerializeField] private float _jumpForce = 1.5f;
    private Vector3 _velocity;


    private PlayerController _controller;
    
    private float _timeScale = 1f;


    private void Start()
    {
        _controller = ServiceLocator.Instance.GetService<PlayerController>();

        _controller.OnJumpEvent += Jump;
    }

    private void Update()
    {
        Move(_controller.Direction);
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
    }

    public void SetTimeScale(float timeScale)
    {
        _timeScale = timeScale;
    }

    public void Move(Vector3 move)
    {
        var direction = _body.forward * move.z + _body.right * move.x;
        _characterController.Move(_localSpeed * Time.deltaTime * direction);
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * -_gravityForce * _timeScale);
        }
    }
}

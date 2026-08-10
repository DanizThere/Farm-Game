using DG.Tweening;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField] private CharacterController _characterController;

    [SerializeField] private bool _enable = true;

    [SerializeField] private float _smooth = 10f;
    [SerializeField] private float _amplitude = .015f;
    [SerializeField] private float _frequency = 10f;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cameraHolderTransform;

    private Vector3 _startPosition;
    private Vector3 _movePosition;

    private void Start()
    {
        _startPosition = _cameraTransform.localPosition;
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();
    }

    private void Update()
    {
        if (!_enable) return;

        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _movePosition, Time.deltaTime * _smooth);

        if (_playerController.Direction.magnitude < .1f)
        {
            _movePosition = _startPosition;
            return;
        }

        if (!_characterController.isGrounded)
        {
            _movePosition = _startPosition;
            return;
        }

        _movePosition = FootstepMotion();
    }

    private Vector3 FootstepMotion()
    {
        var pos = _startPosition;

        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency * .5f) * _amplitude * 2;
        return pos;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicObject : MonoBehaviour, ISelectable
{
    public float CurrentWeight => _currentWeight;

    [Range(0.1f, 1f)]
    [SerializeField] private float _defaultWeight = .5f;
    [SerializeField] private float _timeForce = 10f;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private float _angleClamp = 30f;
    private float _currentWeight;
    private Rigidbody _rb;
    private CameraHandler _handler;

    private Vector3 _velocity;
    private bool _isGrabbed = false;
    private List<HandController> _hands = new();

    private void Awake()
    {
        _currentWeight = _defaultWeight;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!_isGrabbed) return;
        if (!_canMove) return;
        _velocity = _handler!.GetForward(2f) + Vector3.down;

        if (_currentWeight > .5f)
        {
            _velocity.y = transform.position.y - Vector3.Distance(Vector3.zero, transform.position) * .1f;
        }

        var hand = _hands[0];
        var diff = hand.transform.rotation * Quaternion.Inverse(hand.DefaultRotation);
        var targetRotation = diff * transform.rotation * Quaternion.AngleAxis(_angleClamp, transform.right);

        var angleClamp = Quaternion.Angle(transform.rotation, targetRotation);

        var targetPosition = Vector3.MoveTowards(transform.position, _velocity, Time.deltaTime * _currentWeight * _timeForce);

        _rb.MovePosition(targetPosition);


        if (angleClamp > _angleClamp)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 5f * _currentWeight * _timeForce);
        }
    }

    public void Grab(float force, CameraHandler handler, out float speedScale, HandController hand)
    {
        if (!_hands.Contains(hand)) _hands.Add(hand);
        _isGrabbed = true;

        if(_handler == null)
        {
            _handler = handler;
        }

        _rb.useGravity = false;
        _rb.interpolation = RigidbodyInterpolation.None;
        _currentWeight = Mathf.Max(0.1f, _currentWeight - force);
        speedScale = 1 - _currentWeight / _defaultWeight;
    }

    public void UnGrab(HandController hand)
    {
        if (_hands.Contains(hand)) _hands.Remove(hand);

        _isGrabbed = _hands.Any();

        _currentWeight = _defaultWeight;
        _rb.useGravity = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public bool CanSelect()
    {
        //throw new System.NotImplementedException();
        return true;
    }

    public void Hide()
    {
        //throw new System.NotImplementedException();
    }

    public void Show()
    {
        //throw new System.NotImplementedException();
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicObject : MonoBehaviour, ISelectable
{
    public float CurrentWeight => _currentWeight;

    [Range(0.1f, 1f)]
    [SerializeField] protected float _defaultWeight = .5f;
    [SerializeField] protected float _timeForce = 10f;
    [SerializeField] protected bool _canMove = true;
    [SerializeField] protected ParticleSystem _collideFX;
    protected float _currentWeight;
    protected Rigidbody _rb;
    protected CameraHandler _handler;

    protected Vector3 _velocity;
    protected bool _isGrabbed = false;
    protected List<HandController> _hands = new();
    private float _emitProgress = 1f;

    private void Awake()
    {
        _currentWeight = _defaultWeight;
        _rb = GetComponent<Rigidbody>();
        _rb.mass = _defaultWeight * 10f;
    }

    public virtual void Move(float angleHandClamp = 30)
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
        var targetRotation = diff * transform.rotation * Quaternion.AngleAxis(angleHandClamp, transform.right);

        var angleClamp = Quaternion.Angle(transform.rotation, targetRotation);

        var targetPosition = Vector3.MoveTowards(transform.position, _velocity, Time.deltaTime * (1 - _currentWeight) * _timeForce);

        _rb.MovePosition(targetPosition);


        if (angleClamp > angleHandClamp)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 5f * _currentWeight * _timeForce);
        }
    }

    public virtual void Grab(float force, CameraHandler handler, out float speedScale, HandController hand)
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
        speedScale = _currentWeight / _defaultWeight;
    }

    public virtual void UnGrab(HandController hand, out float speedScale)
    {
        if (_hands.Contains(hand)) _hands.Remove(hand);

        _isGrabbed = _hands.Any();

        _currentWeight = _defaultWeight;
        speedScale = _currentWeight / _defaultWeight;
        if (!_isGrabbed)
        {
            _rb.useGravity = true;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        print($"{gameObject.name} speed: {_rb.linearVelocity.normalized.sqrMagnitude}");
        if(_rb.linearVelocity.normalized.sqrMagnitude > .5f)
        {
            EmitOnStay(collision.contacts.FirstOrDefault().point);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EmitOnEnter(collision.contacts.FirstOrDefault().point);
    }

    private void EmitOnEnter(Vector3 point)
    {
        _collideFX.transform.position = point;

        _collideFX?.Play();
    }

    private void EmitOnStay(Vector3 point)
    {
        _collideFX.transform.position = point;

        if (_emitProgress > 1f)
        {
            _collideFX?.Play();
            _emitProgress = 0f;
            return;
        }

        _emitProgress += Time.deltaTime;
    }
}

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
    [SerializeField] protected float _massMultiplier = 10f;
    [SerializeField] protected float _maxGrabSpeed = 8f;
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
        _rb.mass = Mathf.Max(0.1f, _defaultWeight * _massMultiplier);

        //if(_defaultWeight > .8f)
        //{
        //    _rb.constraints = RigidbodyConstraints.FreezePositionY;
        //}
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public virtual void Move(float angleHandClamp = 30)
    {
        if (!_isGrabbed) return;
        if (!_canMove) return;
        if (_hands.Count == 0) return;
        if (_handler == null) return;

        var targetPoint = _handler.GetForward(2f) + Vector3.down * _currentWeight;
        var weightFactor = Mathf.Clamp01(1f - _currentWeight);

        var toTarget = targetPoint - _rb.position;
        var desiredVelocity = toTarget * _timeForce * weightFactor;
        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, _maxGrabSpeed);

        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, desiredVelocity, Time.fixedDeltaTime * 20f);
        _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);

        var hand = _hands[0];
        var diff = hand.transform.rotation * Quaternion.Inverse(hand.DefaultRotation);
        var targetRotation = diff * transform.rotation * Quaternion.AngleAxis(angleHandClamp, transform.right);
        var angleClamp = Quaternion.Angle(transform.rotation, targetRotation);

        if (angleClamp > angleHandClamp)
        {
            var targetRot = Quaternion.RotateTowards(_rb.rotation, targetRotation, Time.fixedDeltaTime * 5f * _currentWeight * _timeForce);
            _rb.MoveRotation(targetRot);
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
        _rb.interpolation = RigidbodyInterpolation.Extrapolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _currentWeight = Mathf.Max(0.1f, _currentWeight - force);

        _rb.mass = Mathf.Max(0.1f, _currentWeight * _massMultiplier);
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

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
        if(_rb.linearVelocity.sqrMagnitude > .5f)
        {
            if (collision.contactCount > 0)
                EmitOnStay(collision.contacts.FirstOrDefault().point);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contactCount == 0) return;

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

    public bool CanSelect()
    {
        var hands = FindObjectsByType<HandController>();

        return hands.Any(x => !x.IsEffortOrObjectBusy);
    }
}

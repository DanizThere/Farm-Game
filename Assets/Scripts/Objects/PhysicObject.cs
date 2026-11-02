using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicObject : MonoBehaviour, ISelectable, ICartLoadable
{
    public float CurrentWeight => _currentWeight;

    public Rigidbody Body => _rb;

    public Transform Root => transform;
    public bool CanMove => _canMove;

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
    protected bool _canGrab = true;
    protected List<HandController> _hands = new();
    private float _emitProgress = 1f;
    protected bool _isLoaded = false;

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        _currentWeight = _defaultWeight;
        _rb = GetComponent<Rigidbody>();
        _rb.mass = Mathf.Max(0.1f, _defaultWeight * _massMultiplier);

        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (!_canMove)
        {
            _rb.useGravity = false;
            _rb.interpolation = RigidbodyInterpolation.Extrapolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rb.isKinematic = true;
        }
    }

    public virtual void Move(float angleHandClamp = 30)
    {
        if (!_canGrab) return;
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

        var point = collision.contacts.FirstOrDefault().point;

        EmitOnEnter(point);
    }

    private void EmitOnEnter(Vector3 point)
    {
        ServiceLocator.Instance.GetService<SFXManager>().Play(point, "OnHit");
        _collideFX.transform.position = point;

        _collideFX?.Play();
    }

    private void EmitOnStay(Vector3 point)
    {
        _collideFX.transform.position = point;

        if (_emitProgress > 1f)
        {
            ServiceLocator.Instance.GetService<SFXManager>().Play(point, "OnHit");

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

    public bool CanGrab()
    {
        return _canGrab;
    }

    public virtual void OnLoaded(CartObject cart)
    {
        if (_isLoaded) return;
        _isLoaded = true;

        if (_rb != null)
        {
            _rb.useGravity = false;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        _canMove = false;
    }

    public virtual void OnUnloaded(CartObject cart)
    {
        if (!_isLoaded) return;
        _isLoaded = false;

        if (_rb != null) _rb.useGravity = true;
        _canMove = true;
    }
}

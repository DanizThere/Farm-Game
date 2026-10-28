using System;
using System.Collections.Generic;
using UnityEngine;

public class CartObject : PhysicObject
{
    [SerializeField] private Collider _loadZone;
    [SerializeField] private float _holdDistance = 4f;

    [SerializeField] private float _loadStickSpeed = 12f;
    [SerializeField] private float _loadMaxSpeed = 6f;
    [Range(0f, 1f)]
    [SerializeField] private float _followStrength = 0.85f;

    [SerializeField] private float _tipOverAngle = 65f;
    [SerializeField] private bool _unloadWhenUpsideDown = true;

    private List<ICartLoadable> _cargo = new();
    private Dictionary<ICartLoadable, Vector3> _cargoLocalSlots = new();
    private HashSet<ICartLoadable> _inZone = new();

    public IReadOnlyList<ICartLoadable> Cargo => _cargo;

    private void FixedUpdate()
    {
        if (_unloadWhenUpsideDown && Vector3.Dot(transform.up, Vector3.up) < -0.2f)
        {
            UnloadAll();
            return;
        }
        if (Vector3.Angle(transform.up, Vector3.up) > _tipOverAngle)
        {
            UnloadAll();
            return;
        }

        for (int i = _cargo.Count - 1; i >= 0; i--)
        {
            var loadable = _cargo[i];
            if (loadable == null) { _cargo.RemoveAt(i); continue; }

            Vector3 worldSlot = _loadZone.transform.TransformPoint(_cargoLocalSlots[loadable]);
            var body = loadable.Body;

            if (body != null)
            {
                Vector3 toSlot = worldSlot - body.position;
                Vector3 desiredVel = Vector3.ClampMagnitude(toSlot * _loadStickSpeed, _loadMaxSpeed);
                Vector3 cartVel = _rb.linearVelocity;
                Vector3 targetVel = Vector3.Lerp(body.linearVelocity, cartVel + desiredVel, _followStrength);

                body.linearVelocity = Vector3.Lerp(
                    body.linearVelocity, targetVel, Time.fixedDeltaTime * 15f);
                body.angularVelocity = Vector3.Lerp(
                    body.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 8f);
            }
            else
            {
                var t = loadable.Root;
                if (t == null) { _cargo.RemoveAt(i); continue; }

                t.position = Vector3.Lerp(t.position, worldSlot, Time.fixedDeltaTime * _loadStickSpeed);
                t.rotation = Quaternion.Slerp(t.rotation, _loadZone.transform.rotation,
                                              Time.fixedDeltaTime * _loadStickSpeed * 0.5f);
            }
        }
    }

    public override void Move(float angleHandClamp = 30)
    {
        if (!_isGrabbed) return;
        if (!_canMove) return;
        if (_hands.Count == 0) return;
        if (_handler == null) return;

        var targetPoint = _handler.GetForward(_holdDistance) + Vector3.down * _currentWeight;
        var weightFactor = Mathf.Clamp01(1f - _currentWeight);
        var loadFactor = Mathf.Clamp01(1f - _currentWeight * 0.15f);

        var toTarget = targetPoint - _rb.position;
        var desiredVelocity = toTarget * _timeForce * weightFactor * loadFactor;
        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, _maxGrabSpeed);

        _rb.linearVelocity = Vector3.Lerp(
            _rb.linearVelocity, desiredVelocity, Time.fixedDeltaTime * 20f);

        StabilizeRotation();
    }

    public void TryRegisterInZone(Collider other)
    {
        if (_loadZone == null || other == _loadZone) return;

        var loadable = other.GetComponentInParent<ICartLoadable>();
        if (loadable == null || ReferenceEquals(loadable, this)) return;
        if (!_inZone.Add(loadable)) return;

        TryLoad(loadable);
    }

    public void UnregisterFromZone(Collider other)
    {
        var loadable = other.GetComponentInParent<ICartLoadable>();
        if (loadable == null) return;
        _inZone.Remove(loadable);

        if (_cargo.Contains(loadable)) Unload(loadable);
    }

    private void StabilizeRotation()
    {
        var euler = transform.eulerAngles;
        var upright = Quaternion.Euler(0f, euler.y, 0f);
        _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, upright, Time.fixedDeltaTime * 3f));
        var velocity = _rb.angularVelocity;
        _rb.angularVelocity = new Vector3(velocity.x * 0.8f, velocity.y, velocity.z * 0.8f);
    }

    public bool TryLoad(ICartLoadable loadable)
    {
        if (loadable == null) return false;
        if (ReferenceEquals(loadable, this)) return false;
        if (_cargo.Contains(loadable)) return true;

        _cargo.Add(loadable);
        _cargoLocalSlots[loadable] = ComputeLocalSlot(loadable);

        loadable.OnLoaded(this);
        return true;
    }

    public void Unload(ICartLoadable loadable)
    {
        if (loadable == null) return;
        if (!_cargo.Remove(loadable)) return;
        _cargoLocalSlots.Remove(loadable);

        loadable.OnUnloaded(this);
    }

    public void UnloadAll()
    {
        for (int i = _cargo.Count - 1; i >= 0; i--) Unload(_cargo[i]);
    }

    private Vector3 ComputeLocalSlot(ICartLoadable loadable)
    {
        int idx = _cargo.IndexOf(loadable);
        int perRow = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(6)));
        int row = idx / perRow;
        int col = idx % perRow;

        Vector3 size = _loadZone.bounds.size;
        float x = perRow > 1 ? (col / (float)(perRow - 1) - 0.5f) * size.x * 0.6f : 0f;
        float z = perRow > 1 ? (row / (float)(perRow - 1) - 0.5f) * size.z * 0.6f : 0f;
        float y = size.y * 0.1f;

        return new Vector3(x, y, z);
    }
}

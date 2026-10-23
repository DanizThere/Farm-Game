using System.Collections.Generic;
using UnityEngine;

public class CartObject : PhysicObject
{
    public override void Move(float angleHandClamp = 30)
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
    }
}

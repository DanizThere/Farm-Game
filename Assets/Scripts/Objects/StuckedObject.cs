using UnityEngine;

public class StuckedObject : PhysicObject
{
    [SerializeField] private float _breakAngle = 45f;
    [SerializeField] private ParticleSystem _breakingFX;
    private Quaternion _startCameraRotation;
    private float _breakProgress;

    private void Start()
    {
        _canMove = false;
        _rb.isKinematic = true;
        _rb.useGravity = false;
    }

    public override void Move(float angleHandClamp = 30)
    {
        if(_breakProgress < 1f)
        {
            var angle = Quaternion.Angle(_handler.Camera.transform.rotation, _startCameraRotation);
            if (angle > _breakAngle)
            {
                _breakProgress += Time.fixedDeltaTime * (2f * _currentWeight) * (angle / 90f);
                _breakingFX.Emit(Mathf.RoundToInt(_breakProgress * 10));

                if(_breakProgress > 1f)
                {
                    _canMove = true;
                }
            }
        }

        if(_canMove)
            base.Move(angleHandClamp);
    }

    public override void Grab(float force, CameraHandler handler, out float speedScale, HandController hand)
    {
        base.Grab(force, handler, out speedScale, hand);

        _startCameraRotation = _handler.Camera.transform.rotation;
    }
}

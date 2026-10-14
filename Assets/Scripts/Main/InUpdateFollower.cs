using UnityEngine;

public class InUpdateFollower : SLMonoBehaviour
{
    public bool ShouldLookAsCamera = false;

    [SerializeField] private Transform _target;
    [SerializeField] private FollowOption _followSpeed;
    [SerializeField] private FollowOption _rotationSpeed;

    private Transform _cameraTransform;

    protected override void SLStart()
    {
        base.SLStart();
        _cameraTransform = ServiceLocator.Instance.GetService<CameraHandler>().Camera.transform ??
            throw new System.NullReferenceException($"{nameof(CameraHandler)} doesn't exist");

        _followSpeed.SetupDiff();
        _rotationSpeed.SetupDiff();
    }

    protected override void SLUpdate()
    {
        if (_target == null) return;

        transform.position = Vector3.Lerp(transform.position, _target.position, _followSpeed.CurrentValue * Time.deltaTime);
        if (ShouldLookAsCamera)
        {
            var targetRotation = _cameraTransform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed.CurrentValue * Time.deltaTime);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}

[System.Serializable]
public class FollowOption
{
    public float Max;
    public float Min;
    public float InitValue;

    public float CurrentValue => InitValue + _diff;

    private float _diff;
    public void SetupDiff()
    {
        _diff = Random.Range(Min, Max);
    }
}

using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public CameraMovement CameraMovement => _cameraMovement;
    public Transform WatchPosition => _watchPosition;
    public Transform LeftHandTransform => _leftHandTransform;
    public Transform RightHandTransform => _rightHandTransform;
    public Camera Camera => _camera;

    [SerializeField] private Transform _watchPosition;
    [SerializeField] private Transform _leftHandTransform;
    [SerializeField] private Transform _rightHandTransform;
    [SerializeField] private InUpdateFollower _inUpdateFollower;

    [SerializeField] private Camera _camera;
    [SerializeField] private CameraMovement _cameraMovement;

    private Transform _defaultTarget;

    public void Initialize(Transform target, Transform body)
    {
        _defaultTarget = target;

        _cameraMovement.Initialize(body);
        _inUpdateFollower.SetTarget(_defaultTarget);
    }

    public void SetTarget(Transform target)
    {
        _inUpdateFollower.SetTarget(target);
    }

    public void ResetTarget()
    {
        _inUpdateFollower.SetTarget(_defaultTarget);
    }
}

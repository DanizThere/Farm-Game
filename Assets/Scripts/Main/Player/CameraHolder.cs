using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public CameraMovement CameraMovement => _cameraMovement;
    public Transform WatchPosition => _watchPosition;
    public Transform LeftHandTransform => _leftHandTransform;
    public Transform RightHandTransform => _rightHandTransform;
    public Camera Camera => _camera;
    public Fading Fading => _fading;

    [SerializeField] private Transform _watchPosition;
    [SerializeField] private Transform _leftHandTransform;
    [SerializeField] private Transform _rightHandTransform;
    [SerializeField] private InUpdateFollower _inUpdateFollower;
    [SerializeField] private Fading _fading;

    [SerializeField] private Camera _camera;
    [SerializeField] private CameraMovement _cameraMovement;

    private Transform _defaultTarget;

    public void Initialize(Transform target, Transform body, Transform model)
    {
        _defaultTarget = target;

        _cameraMovement.Initialize(body, model);
        _inUpdateFollower.SetTarget(_defaultTarget);
        _fading.UnFade();
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

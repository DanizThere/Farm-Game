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

    public void Initialize(Transform target, Transform body)
    {
        _cameraMovement.Initialize(body);
        _inUpdateFollower.SetTarget(target);
    }
}

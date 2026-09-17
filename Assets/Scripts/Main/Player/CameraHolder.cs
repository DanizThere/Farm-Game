using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform LeftHandTransform => _leftHandTransform;
    public Transform RightHandTransform => _rightHandTransform;
    public Camera Camera => _camera;

    [SerializeField] private Transform _leftHandTransform;
    [SerializeField] private Transform _rightHandTransform;
    [SerializeField] private InUpdateFollower _inUpdateFollower;

    [SerializeField] private Camera _camera;

    public void Initialize(Transform target, Transform body)
    {
        GetComponentInChildren<CameraMovement>().Initialize(body);
        _inUpdateFollower.SetTarget(target);
    }
}

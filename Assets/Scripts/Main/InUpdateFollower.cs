using UnityEngine;

public class InUpdateFollower : MonoBehaviour
{
    public bool ShouldLookAsCamera = false;

    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed = 10;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = ServiceLocator.Instance.GetService<CameraHandler>()?.Camera.transform;
        _cameraTransform = _cameraTransform != null ? _cameraTransform : Camera.main.transform;
    }

    private void Update()
    {
        if (_target == null || !_cameraTransform) return;
        transform.position = Vector3.Lerp(transform.position, _target.position, _followSpeed * Time.deltaTime);
        if (ShouldLookAsCamera)
        {
            var targetRotation = _cameraTransform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _followSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}

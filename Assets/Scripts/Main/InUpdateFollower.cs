using UnityEngine;

public class InUpdateFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private bool _shouldLookAsCamera = false;
    [SerializeField] private float _followSpeed = 10;

    private CameraHandler _camera;

    private void Start()
    {
        _camera = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _followSpeed * Time.deltaTime);
        if (_shouldLookAsCamera)
        {
            var targetRotation = _camera.Camera.transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _followSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}

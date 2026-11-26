using UnityEngine;
using UnityEngine.ProBuilder;

public class UITilt : MonoBehaviour
{
    [SerializeField] private Vector2 _maxOffset = new Vector2(30f, 20f);
    [SerializeField] private float _maxTilt = 4f;
    [SerializeField] private float _sensitivity = 1f;
    [SerializeField] private float _degreesForFullOffset = 45f;
    [SerializeField] private float _returnSpeed = 6f;
    [SerializeField] private float _followSpeed = 10f;
    [SerializeField] private bool _clampOffset = true;

    private RectTransform rect;
    private Vector2 basePosition;
    private float baseRotationZ;

    private Vector2 currentOffset;
    private float currentTilt;
    private Transform _cameraTransform;

    private Quaternion lastCameraRotation;
    private Vector2 targetOffset;
    private float targetTilt;

    private void Start()
    {
        rect = transform as RectTransform;
        _cameraTransform = ServiceLocator.Instance.GetService<CameraHandler>().Camera.transform;

        basePosition = rect.anchoredPosition;
        baseRotationZ = rect.localEulerAngles.z;

        if (_cameraTransform != null)
            lastCameraRotation = _cameraTransform.rotation;
    }

    void LateUpdate()
    {
        if (_cameraTransform == null) return;

        Quaternion deltaRot = _cameraTransform.rotation * Quaternion.Inverse(lastCameraRotation);
        Vector3 deltaEuler = deltaRot.eulerAngles;

        deltaEuler.x = NormalizeAngle(deltaEuler.x);
        deltaEuler.y = NormalizeAngle(deltaEuler.y);
        deltaEuler.z = NormalizeAngle(deltaEuler.z);
        lastCameraRotation = _cameraTransform.rotation;

        float yaw = NormalizeAngle(_cameraTransform.eulerAngles.y);
        float pitch = NormalizeAngle(_cameraTransform.eulerAngles.x);

        float normX = Mathf.Clamp(yaw / _degreesForFullOffset, -1f, 1f);
        float normY = Mathf.Clamp(pitch / _degreesForFullOffset, -1f, 1f);

        targetOffset = new Vector2(normX * _maxOffset.x, normY * _maxOffset.y) * _sensitivity;
        targetTilt = -normX * _maxTilt * _sensitivity;

        float t = 1f - Mathf.Exp(-_followSpeed * Time.deltaTime);
        currentOffset = Vector2.Lerp(currentOffset, targetOffset, t);
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, t);

        bool cameraIdle = Mathf.Abs(deltaEuler.x) < 0.01f &&
                          Mathf.Abs(deltaEuler.y) < 0.01f &&
                          Mathf.Abs(deltaEuler.z) < 0.01f;

        if (cameraIdle)
        {
            float back = 1f - Mathf.Exp(-_returnSpeed * Time.deltaTime);
            currentOffset = Vector2.Lerp(currentOffset, Vector2.zero, back);
            currentTilt = Mathf.Lerp(currentTilt, 0f, back);
        }

        Vector2 finalOffset = basePosition + currentOffset;
        if (_clampOffset)
        {
            finalOffset.x = Mathf.Clamp(finalOffset.x,
                basePosition.x - _maxOffset.x * Mathf.Abs(_sensitivity),
                basePosition.x + _maxOffset.x * Mathf.Abs(_sensitivity));
            finalOffset.y = Mathf.Clamp(finalOffset.y,
                basePosition.y - _maxOffset.y * Mathf.Abs(_sensitivity),
                basePosition.y + _maxOffset.y * Mathf.Abs(_sensitivity));
        }

        rect.anchoredPosition = finalOffset;
        rect.localRotation = Quaternion.Euler(0f, 0f, baseRotationZ + currentTilt);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    public void ResetToBase()
    {
        currentOffset = Vector2.zero;
        currentTilt = 0f;
        targetOffset = Vector2.zero;
        targetTilt = 0f;
        if (rect != null)
        {
            rect.anchoredPosition = basePosition;
            rect.localRotation = Quaternion.Euler(0f, 0f, baseRotationZ);
        }
    }

    public void SetBasePosition(Vector2 newBase)
    {
        basePosition = newBase;
    }
}
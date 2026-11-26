using UnityEngine;

public class UIBobbing : MonoBehaviour
{
    public bool isRunning = false;
    public bool EnableBobbing = true;

    [SerializeField] private float _bobAmountX = 0.02f;
    [SerializeField] private float _bobAmountY = 0.03f;
    [SerializeField] private float _bobSpeed = 6f;

    [SerializeField] private float _runAmountMultiplier = 1.8f;
    [SerializeField] private float _runSpeedMultiplier = 1.7f;
    [SerializeField] private float _runTransitionSpeed = 6f;

    [SerializeField] private float _tiltAmountX = 2f;
    [SerializeField] private float _tiltAmountZ = 3f;

    [SerializeField] private float _returnSmoothness = 8f;

    private Vector3 _initialLocalPosition;
    private Quaternion _initialLocalRotation;

    private Vector3 _targetPositionOffset;
    private Quaternion _targetRotationOffset = Quaternion.identity;

    private float _time;
    private float _runBlend = 0f;

    private RectTransform _rect;

    private void Start()
    {
        _rect = transform as RectTransform;

        _initialLocalPosition = _rect.anchoredPosition;

        _time = Random.Range(0f, Mathf.PI * 2f);
    }

    private void LateUpdate()
    {
        float targetRunBlend = isRunning ? 1f : 0f;
        _runBlend = Mathf.MoveTowards(_runBlend, targetRunBlend, Time.deltaTime * _runTransitionSpeed);

        float currentSpeedMultiplier = Mathf.Lerp(1f, _runSpeedMultiplier, _runBlend);
        float currentAmountMultiplier = Mathf.Lerp(1f, _runAmountMultiplier, _runBlend);

        _time += Time.deltaTime * currentSpeedMultiplier;

        if (EnableBobbing)
        {
            float sinX = Mathf.Sin(_time * _bobSpeed);
            float sinY = Mathf.Sin(_time * _bobSpeed * 2f);

            _targetPositionOffset = new Vector3(
                sinX * _bobAmountX * currentAmountMultiplier,
                sinY * _bobAmountY * currentAmountMultiplier,
                0f
            );
            Quaternion tiltX = Quaternion.Euler(sinY * _tiltAmountX * currentAmountMultiplier, 0f, 0f);
            Quaternion tiltZ = Quaternion.Euler(0f, 0f, sinX * _tiltAmountZ * currentAmountMultiplier);
            _targetRotationOffset = tiltX * tiltZ;
        }
        else
        {
            _targetPositionOffset = Vector3.zero;
            _targetRotationOffset = Quaternion.identity;
        }

        _rect.anchoredPosition = _initialLocalPosition + _targetPositionOffset;

        //transform.localRotation = Quaternion.Slerp(
        //    transform.localRotation,
        //    _initialLocalRotation * _targetRotationOffset,
        //    Time.deltaTime * _returnSmoothness
        //);
    }
}

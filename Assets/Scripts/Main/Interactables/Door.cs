using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float _openSmooth = 10f;
    [SerializeField] private float _openAngle = 90f;
    [SerializeField] private Transform _doorTransform;
    private bool _isOpen = false;
    private Coroutine _coroutine;
    private float _currentAngle;

    public void Open(float dot)
    {
        var targetAngle = dot < 0f ? -_openAngle : _openAngle;
        if (_isOpen)
            targetAngle = 0f;

        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(RotateTo(targetAngle));
        _isOpen = !_isOpen;
    }

    private IEnumerator RotateTo(float targetAngle)
    {
        while (Mathf.Abs(_currentAngle - targetAngle) > 0.01f)
        {
            _currentAngle = Mathf.Lerp(
                _currentAngle,
                targetAngle,
                1f - Mathf.Exp(-_openSmooth * Time.deltaTime)
            );

            _doorTransform.localRotation = Quaternion.Euler(0f, _currentAngle, 0f);
            yield return null;
        }

        _currentAngle = targetAngle;
        _doorTransform.localRotation = Quaternion.Euler(0f, _currentAngle, 0f);
        _coroutine = null;
    }
}

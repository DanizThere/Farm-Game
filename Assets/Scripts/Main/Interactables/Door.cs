using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float _openSmooth = 10f;
    [SerializeField] private float _openAngle = 90f;
    [SerializeField] private Transform _doorTransform;
    private bool _isOpen = false;
    private IEnumerator _coroutine;
    private float _currentAngle;

    public void Open(float dot)
    {
        if(_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = OpenAsync(dot);

        StartCoroutine(_coroutine);
        _isOpen = !_isOpen;
    }

    private IEnumerator OpenAsync(float value)
    {
        var angle = 0f;

        if (_isOpen)
        {
            angle = _currentAngle;

            while (!Mathf.Approximately(angle, 0f))
            {
                yield return null;

                angle = Mathf.Lerp(angle, 0, Time.deltaTime * _openSmooth);
                var rotation = Quaternion.Euler(0, angle, 0);
                _doorTransform.rotation = rotation;
            }
            yield break;
        }


        if (value < 0)
        {
            while(angle >= -_openAngle)
            {
                yield return null;

                angle = Mathf.Lerp(angle, -_openAngle, Time.deltaTime * _openSmooth);
                _currentAngle = angle;
                var rotation = Quaternion.Euler(0, angle, 0);
                _doorTransform.rotation = rotation;
            }
        }
        else if(value > 0)
        {
            while (angle <= _openAngle)
            {
                yield return null;

                angle = Mathf.Lerp(angle, _openAngle, Time.deltaTime * _openSmooth);
                _currentAngle = angle;
                var rotation = Quaternion.Euler(0, angle, 0);
                _doorTransform.rotation = rotation;
            }
        }
    }
}

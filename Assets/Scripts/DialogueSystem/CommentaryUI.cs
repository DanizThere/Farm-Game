using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class CommentaryUI : View
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _hideDuration = 2f;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Setup(string message, Vector3 startPosition, float duration)
    {
        transform.position = startPosition;

        Vector3 direction = ServiceLocator.Instance.GetService<CameraHandler>().Camera.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        _text.text = message;
        Move(duration);
    }

    private void Move(float duration)
    {
        transform
            .DOMoveY(transform.position.y + 1, duration)
            .OnComplete(() => _canvasGroup.DOFade(0, _hideDuration)
            .OnComplete(() => Destroy(gameObject)));
    }
}

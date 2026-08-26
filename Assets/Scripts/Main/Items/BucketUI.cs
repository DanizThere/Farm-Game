using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BucketUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fillAmount;

    [SerializeField] private InUpdateFollower _follower;

    private Tween _currentTween;

    public void Setup(Transform target, float currentProgress)
    {
        _follower.SetTarget(target);
        _follower.ShouldLookAsCamera = true;
        ShowDurability(currentProgress);
    }

    public void ShowDurability(float progress)
    {
        var color = _gradient.Evaluate(progress);

        _fillAmount.fillAmount = progress;
        _fillAmount.color = color;
    }

    public void Show()
    {
        _currentTween?.Kill();
        _currentTween = ShowTween();

        _currentTween.Play();
    }

    public void Hide()
    {
        _currentTween?.Kill();
        _currentTween = HideTween();

        _currentTween.Play();

    }

    private Tween ShowTween()
    {
        _panel.SetActive(true);
        var rect = _panel.GetComponent<RectTransform>();
        return rect.DOScaleY(1, .5f).From(.2f);
    }

    private Tween HideTween()
    {
        var rect = _panel.GetComponent<RectTransform>();
        return rect.DOScaleY(.2f, .25f).OnComplete(() => Destroy(gameObject));
    }
}
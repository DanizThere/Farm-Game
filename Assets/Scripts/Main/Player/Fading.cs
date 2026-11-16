using DG.Tweening;
using UnityEngine;

public class Fading : MonoBehaviour
{
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private float _duration;

    public void Fade()
    {
        _group.gameObject.SetActive(true);
        _group.DOFade(1, _duration);
    }

    public void UnFade()
    {
        _group.DOFade(0, _duration).OnComplete(() => _group.gameObject.SetActive(false));
    }
}

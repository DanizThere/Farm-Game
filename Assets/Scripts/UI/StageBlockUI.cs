using UnityEngine;

public class StageBlockUI : MonoBehaviour
{
    public DayStage Stage;
    public float NextPosition => _startPosition + _width;

    private RectTransform _transform;

    private float _startPosition;
    private float _width;

    private void Awake()
    {
        _transform = transform as RectTransform;
        _width = _transform.rect.width;
    }

    public void SetWidth(float width)
    {
        _width = width;
    }

    public void SetPosition(float startPoint)
    {
        _startPosition = startPoint;

        var sizeDelta = _transform.sizeDelta;
        sizeDelta.x = _width;
       
        _transform.sizeDelta = sizeDelta;
        _transform.anchoredPosition = new(_startPosition, 0);
    }
}

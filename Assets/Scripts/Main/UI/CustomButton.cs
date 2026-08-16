using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnPointerDownEvent = new();
    public UnityEvent OnClickEvent = new();

    private bool _isHolding = false;

    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if(_isHolding)
        {
            OnPointerDownEvent?.Invoke();

        }
    }

    private Tween OnHoverEnter()
    {
        var rect = _button.GetComponent<RectTransform>();
        return rect.DOScale(1.2f, .5f);
    }

    private Tween OnHoverExit()
    {
        var rect = _button.GetComponent<RectTransform>();
        return rect.DOScale(1f, .5f);
    }

    private void OnClick()
    {
        OnClickEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var tween = OnHoverEnter();

        tween.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var tween = OnHoverExit();

        tween.Play();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        var rect = GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            eventData.position,
            Camera.main,
            out var localMousePosition
        );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public UnityEvent OnClickEvent = new();

    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private Tween OnHoverEnter()
    {
        return _button.transform.DOScale(1.2f, .5f);
    }

    private Tween OnHoverExit()
    {
        return _button.transform.DOScale(1f, .5f);
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

        print(localMousePosition);
    }
}

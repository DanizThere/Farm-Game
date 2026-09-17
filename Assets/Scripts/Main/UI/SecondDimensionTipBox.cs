using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecondDimensionTipBox : View
{

    [SerializeField] private RectTransform _dragParent;
    [SerializeField] private RectTransform _dragTransform;

    [SerializeField] private TextMeshProUGUI _nameText, _descriptionText;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (IsActive)
        {
            var position = Mouse.current.position.ReadValue();
            Move(position);
        }
    }

    public override void Hide()
    {
        _isActive = false;
        _dragTransform.gameObject.SetActive(IsActive);
        SetInfo(string.Empty, string.Empty);
    }

    public override void Show()
    {
        _isActive = true;
        _dragTransform.gameObject.SetActive(IsActive);
    }

    public void SetInfo(string name, string description)
    {
        _nameText.text = name;
        _descriptionText.text = description;
    }

    public void Move(Vector2 position)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
                (_dragParent,
                position,
                null,
                out var localMousePosition))
        {
            print(localMousePosition);
            _dragTransform.anchoredPosition = localMousePosition;
        }
    }
}

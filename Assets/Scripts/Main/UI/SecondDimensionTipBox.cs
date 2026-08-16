using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecondDimensionTipBox : MonoBehaviour, IView
{
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;

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

    public void Hide()
    {
        IsActive = false;
        _dragTransform.gameObject.SetActive(IsActive);
        SetInfo(string.Empty, string.Empty);
    }

    public void Show()
    {
        IsActive = true;
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

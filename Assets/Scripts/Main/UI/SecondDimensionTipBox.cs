using TMPro;
using UnityEngine;

public class SecondDimensionTipBox : MonoBehaviour, IView
{
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    [SerializeField] private RectTransform _dragParent;
    [SerializeField] private RectTransform _dragTransform;

    [SerializeField] private TextMeshProUGUI _nameText, _descriptionText;

    private CameraHandler _cameraHandler;
    private ViewController _viewController;

    private void Start()
    {
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
        _viewController = ServiceLocator.Instance.GetService<ViewController>();

        _viewController.Add(this);
        Hide();
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
                _cameraHandler.Camera,
                out var localMousePosition))
        {
            _dragTransform.anchoredPosition = localMousePosition;
        }
    }
}

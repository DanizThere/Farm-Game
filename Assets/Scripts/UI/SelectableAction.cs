using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectableAction : MonoBehaviour
{
    public string ActionName => _actionName;
    public int Order => _order;
    public UnityEvent OnClickEvent = new();

    [SerializeField] private Button _button;
    [SerializeField] private int _order;
    [SerializeField] private string _actionName;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnClickEvent.Invoke();
    }
}

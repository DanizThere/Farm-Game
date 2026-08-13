using TMPro;
using UnityEngine;

public class TimeView : MonoBehaviour, IView
{
    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    [SerializeField] private GameObject _panel;

    [SerializeField] private TextMeshProUGUI _dateText, _timeText;

    private void Start()
    {
        ServiceLocator.Instance.GetService<ViewController>().Add(this);
    }

    public void Hide()
    {
        IsActive = false;
        _panel.SetActive(IsActive);
    }

    public void Show()
    {
        IsActive = true;
        _panel.SetActive(IsActive);
    }

    public void SetTimeText(string time)
    {
        _timeText.text = time;
    }

    public void SetDateText(string date)
    {
        _dateText.text = date;
    }
}

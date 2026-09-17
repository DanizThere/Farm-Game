using TMPro;
using UnityEngine;

public class TimeView : View
{
    [SerializeField] private GameObject _panel;

    [SerializeField] private TextMeshProUGUI _dateText, _timeText;

    public override void Hide()
    {
        _isActive = false;
        _panel.SetActive(IsActive);
    }

    public override void Show()
    {
        _isActive = true;
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

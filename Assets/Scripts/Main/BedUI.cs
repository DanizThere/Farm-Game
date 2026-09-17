using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BedUI : View
{

    [SerializeField] private GameObject _panel;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    private Bed _bed;

    private float _sleepHour;

    public void Setup(Bed bed)
    {
        _bed = bed;
        _slider.onValueChanged.AddListener(SetText);
        Hide();
    }

    public override void Hide()
    {
        _isActive = false;
        _panel.SetActive(IsActive);
        _slider.value = 1;
        _text.text = string.Empty;
        _bed.HideActions();
    }

    public override void Show()
    {
        _isActive = true;
        _panel.SetActive(IsActive);
    }

    public void Sleep()
    {
        if (_sleepHour == 0) return;

        var value = _slider.value;
        _slider.value = 1;
        _bed.SkipHours(value);
        _bed.HideActions();
        Hide();
    }

    private void SetText(float value)
    {
        _sleepHour = value;
        var sleepTime = _bed.GetPossibleTime(value);

        _text.text = $"+{value} hours. You awake about a {sleepTime:dd.MM.yyyy. hh:mm}";
    }
}

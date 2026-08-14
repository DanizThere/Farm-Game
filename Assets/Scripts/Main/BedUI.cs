using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BedUI : MonoBehaviour, IView
{
    public int Order { get; set; } = 20;
    public bool IsActive { get; set; } = false;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Bed _bed;

    private GameStateMachine _gameStateMachine;

    private float _sleepHour;

    private void Start()
    {
        _slider.onValueChanged.AddListener(SetText);
        ServiceLocator.Instance.GetService<ViewController>().Add(this);
        _gameStateMachine = ServiceLocator.Instance.GetService<GameStateMachine>();
        Hide();
    }

    public void Hide()
    {
        IsActive = false;
        _panel.SetActive(IsActive);
        _slider.value = 1;
        _text.text = string.Empty;
        _bed.HideActions();
        _gameStateMachine.SetState<OuterWorldGameState>();
    }

    public void Show()
    {
        IsActive = true;
        _panel.SetActive(IsActive);
        _gameStateMachine.SetState<UIGameState>();
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

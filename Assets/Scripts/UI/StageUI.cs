using System.Linq;
using TMPro;
using UnityEngine;

public class StageUI : View
{
    [SerializeField] private TimeSettings _timeSettings;
    [SerializeField] private TextMeshProUGUI _stageText;
    [SerializeField] private StageBlockUI[] _blocks;
    [SerializeField] private RectTransform _clockTransform;
    [SerializeField] private RectTransform _stageTransform;
    [SerializeField] private float _clockOffset;

    private TimeManager _timeManager;
    private float _width;

    private void Start()
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        _blocks = _blocks.OrderBy(x => x.Stage).ToArray();

        _width = _stageTransform.rect.width - _clockOffset;

        RecalculateBlocksPosition();
    }

    private void Update()
    {
        _stageText.text = $"Stage: {_timeManager.CurrentStage}";
        var position = _timeManager.DayProgress * _width;

        var rect = _clockTransform.anchoredPosition;
        rect.x = position;

        _clockTransform.anchoredPosition = rect;
    }

    private void RecalculateBlocksPosition()
    {
        var nextPosition = 0f;
        for(int i = 0; i < _blocks.Length; i++)
        {
            var newWidth = GetWidth(i) * (_width + _clockOffset);

            _blocks[i].SetWidth(newWidth);
            _blocks[i].SetPosition(nextPosition);

            nextPosition = _blocks[i].NextPosition;
        }
    }

    private float GetWidth(int index)
    {
        switch (index)
        {
            case 0:
                return _timeSettings.SunriseHour / 24f;

            case 1:
                return (_timeSettings.SunStayHour - _timeSettings.SunriseHour) / 24f;

            case 2:
                return (_timeSettings.EveningHour - _timeSettings.SunStayHour) / 24f;
        }

        return (24 - _timeSettings.EveningHour) / 24f;
    }
}
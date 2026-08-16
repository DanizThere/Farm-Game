using System;
using UnityEngine;

public class Bed : DefaultSelectable
{
    [SerializeField] private BedUI _bedUI;

    private const string _sleepAction = "SleepAction";

    private TimeManager _timeManager;

    public override void InitializeInStart()
    {
        base.InitializeInStart();
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();

        foreach (var action in _actions)
        {
            if (action.ActionName == _sleepAction)
            {
                action.OnClickEvent.AddListener(_bedUI.Show);
                continue;
            }
        }

        _bedUI.Setup(this);
    }


    public void SkipHours(float hours)
    {
        _timeManager.SkipHours(hours);
        _gameStateMachine.SetState<OuterWorldGameState>();
    }

    public DateTime GetPossibleTime(float hours)
    {
        return _timeManager.GetDate().AddHours(hours);
    }
}

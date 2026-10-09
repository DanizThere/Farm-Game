using System;
using UnityEngine;

public class Bed : DefaultSelectable
{
    [SerializeField] private BedUI _bedUI;

    private const string _sleepAction = "SleepAction";

    private TimeManager _timeManager;

    protected override void SLStart()
    {
        base.SLStart();
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();

        _bedUI.Setup(this);
    }


    public void SkipHours(float hours)
    {
        _timeManager.SkipHours(hours);
    }

    public DateTime GetPossibleTime(float hours)
    {
        return _timeManager.GetDate().AddHours(hours);
    }
}

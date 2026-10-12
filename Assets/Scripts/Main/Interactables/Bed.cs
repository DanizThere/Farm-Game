using System;
using UnityEngine;

public class Bed : DefaultSelectable
{
    private BedUI _bedUI;
    private TimeManager _timeManager;

    protected override void SLStart()
    {
        base.SLStart();
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>() ??
            throw new NullReferenceException($"{nameof(TimeManager)} doens't exist");
    }


    public void SkipHours(float hours)
    {
        _timeManager.SkipHours(hours);
    }

    public DateTime GetPossibleTime(float hours)
    {
        return _timeManager.GetDate().AddHours(hours);
    }

    public void Show()
    {
        var viewController = ServiceLocator.Instance.GetService<ViewController>();

        var prefab = viewController.Get<BedUI>();
        _bedUI = Instantiate(_bedUI, viewController.Canvas2D.transform);
    }
}

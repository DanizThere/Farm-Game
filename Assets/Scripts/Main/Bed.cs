using System;
using UnityEngine;

public class Bed : MonoBehaviour
{
    private TimeManager _timeManager;

    private void Start()
    {
        _timeManager = ServiceLocator.Instance.GetService<TimeManager>();
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

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Schedule
{
    public DayStage StartStage;
    public ActivityType ActivityType;
}

public enum ActivityType
{
    Idle,
    Socialize,
    Work,
    Sleep
}

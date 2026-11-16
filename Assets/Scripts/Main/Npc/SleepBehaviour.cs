using UnityEngine;
using UnityEngine.AI;

public class SleepBehaviour : IActivity
{
    public ActivityType ActivityType => ActivityType.Sleep;

    private Transform _sleepPosition;

    public SleepBehaviour(Transform sleepPosition)
    {
        _sleepPosition = sleepPosition;
    }

    public void Enter(NPCSensor sensor)
    {
        if (_sleepPosition != null) sensor.Agent.SetDirectionSafe(_sleepPosition);
    }

    public void Exit(NPCSensor sensor)
    {
        sensor.Agent.ResetPath();
    }

    public bool Tick(NPCSensor sensor, float deltaTime)
    {
        return false;
    }
}
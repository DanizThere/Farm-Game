using UnityEngine.AI;

public class IdleBehaviour : IActivity
{
    public ActivityType ActivityType => ActivityType.Idle;

    public void Enter(NPCSensor sensor)
    {
        
    }

    public void Exit(NPCSensor sensor)
    {
        
    }

    public bool Tick(NPCSensor sensor, float deltaTime)
    {
        return true;
    }
}
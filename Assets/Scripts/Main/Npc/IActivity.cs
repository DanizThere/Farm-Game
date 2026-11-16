using UnityEngine;
using UnityEngine.AI;

public interface IActivity
{
    public ActivityType ActivityType { get; }
    public bool Tick(NPCSensor sensor, float deltaTime);
    public void Enter(NPCSensor sensor);
    public void Exit(NPCSensor sensor);
}

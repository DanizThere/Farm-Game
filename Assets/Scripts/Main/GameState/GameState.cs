using UnityEngine;

public abstract class GameState : ScriptableObject
{
    public event System.Action OnStartEvent = delegate { };
    public event System.Action OnUpdateEvent = delegate { };
    public event System.Action OnExitEvent = delegate { };
    public virtual void OnEnter()
    {
        OnStartEvent?.Invoke();
    }
    
    public virtual void OnExit()
    {
        OnExitEvent?.Invoke();
    }

    public virtual void OnUpdate()
    {
        OnUpdateEvent?.Invoke();
    }
}

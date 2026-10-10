using UnityEngine;

[RequireComponent(typeof(PhysicObject))]
public abstract class EffortModel : MonoBehaviour 
{
    public PhysicObject PhysicObject => _physicObject;

    protected PhysicObject _physicObject;

    [SerializeField] protected Transform _modelTransform;
    protected GameObject _model;
    protected Effort _effort;

    public virtual void AddHand(HandController hand)
    {
        _effort?.AddHand(hand);
    }

    public virtual void RemoveHand(HandController hand)
    {
        _effort?.RemoveHand(hand);
    }

    public abstract void ClearModel();
    public abstract void Effort();
    public abstract void SetEffort(Effort effort);
    public abstract void SetModel(GameObject model);
}
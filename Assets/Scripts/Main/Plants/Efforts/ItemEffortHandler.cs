using System;
using UnityEngine;

public abstract class ItemEffortHandler : SLMonoBehaviour
{
    [SerializeField] protected Collider _collider;
    [SerializeField] protected GameObject _handleObject;
    [SerializeField] protected Dirt _dirt;

    protected Effort _effort;
    protected bool _canEffort = true;

    protected override void SLStart()
    {
        base.SLStart();

        var context = ServiceLocator.Instance.GetService<GameDbContext>() ??
            throw new NullReferenceException();

        context.ItemEffortHandlers.Add(this);
    }


    public abstract void ResetEffort();
    public abstract void ShowHandle();
    public abstract void HideHandle();
    public abstract void TryEffort(ItemObject itemObject, Action onSuccess);
}
using System.Linq;
using UnityEngine;

public class Hoe : ItemObject
{
    [SerializeField] private Collider[] _colliders;
    private CameraHandler _handler;
    protected override void SLStart()
    {
        base.SLStart();
        _handler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    public override void AltUse()
    {
        throw new System.NotImplementedException();
    }

    public override void HideItem()
    {
        _gameDbContext.ItemEffortHandlers.Action(x =>
        {
            x.ResetEffort();
        });
    }

    public override void OnPickup(HandController handController)
    {
        _currentHand = handController;

        _rb.useGravity = false;
        _rb.isKinematic = true;
        
        foreach(var collider in _colliders)
        {
            collider.isTrigger = true;
        }
        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>();

        var handlers = _gameDbContext.ItemEffortHandlers.Get(x => x is HoeHandler);
        foreach(var handler in handlers)
        {
            handler.ShowHandle();
        }
    }

    public override void OnThrow()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;

        foreach (var collider in _colliders)
        {
            collider.isTrigger = false;
        }

        _gameDbContext.ItemEffortHandlers.Action(x =>
        {
            x.ResetEffort();
        });
    }

    public override void ShowItem()
    {
        var handlers = _gameDbContext.ItemEffortHandlers.Get(x => x is HoeHandler);
        foreach (var handler in handlers)
        {
            handler.ShowHandle();
        }
    }

    public override void StopAltUse()
    {
        
    }

    public override void StopUse()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if(_handler)
            Debug.DrawRay(_handler.Position, _handler.GetDirection(), Color.red, 100);
    }

    public override void Use()
    {
        if (Physics.Raycast(_handler.Position, _handler.GetDirection(), out var hit, _currentHand.HandDistance))
        {
            if (hit.transform.TryGetComponent<HoeHandler>(out var handler))
            {
                handler.TryEffort(this, () =>
                {
                    _currentHand.ClearItem();
                    _currentHand = null;
                });
            }
        }
    }
}

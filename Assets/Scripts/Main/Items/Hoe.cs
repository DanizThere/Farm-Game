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

    public override void HideItem()
    {
        HideEffort();
    }

    private void HideEffort()
    {
        var handlers = _gameDbContext.ItemEffortHandlers.Get(x => x is HoeHandler);
        foreach (var handler in handlers)
        {
            handler.HideHandle();
        }
    }

    public override void OnPickup(HandController handController)
    {
        _currentHand = handController;

        _rb.useGravity = false;
        _rb.isKinematic = true;

        foreach (var collider in _colliders)
        {
            collider.isTrigger = true;
        }
        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>();
        ShowHandle();
    }

    private void ShowHandle()
    {
        var handlers = _gameDbContext.ItemEffortHandlers.Get(x => x is HoeHandler);
        foreach (var handler in handlers)
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
        HideEffort();
    }

    public override void ShowItem()
    {
        ShowHandle();
    }

    public override void StopUse()
    {

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
                    _gameDbContext.ItemEffortHandlers.Action(x =>
                    {
                        x.ResetEffort();
                    });
                    HideEffort();

                    _currentHand = null;
                });
            }
        }
    }
}

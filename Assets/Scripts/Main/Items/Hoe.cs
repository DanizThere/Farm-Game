using System.Linq;
using UnityEngine;

public class Hoe : ToolItemObject
{
    public override void HideItem()
    {
        base.HideItem();
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
        base.OnPickup(handController);
        ShowHandle();
    }

    private void ShowHandle()
    {
        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>();
        var handlers = _gameDbContext.ItemEffortHandlers.Get(x => x is HoeHandler);
        foreach (var handler in handlers)
        {
            handler.ShowHandle();
        }
    }

    public override void OnThrow()
    {
        base.OnThrow();

        _gameDbContext.ItemEffortHandlers.Action(x =>
        {
            x.ResetEffort();
        });
        HideEffort();
    }

    public override void ShowItem()
    {
        base.ShowItem();
        ShowHandle();
    }

    public override void StopUse()
    {

    }

    public override void Use()
    {
        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit, _currentHand.HandDistance))
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

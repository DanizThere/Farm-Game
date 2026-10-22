using UnityEngine;

public class Showel : ToolItemObject
{

    protected override void SLStart()
    {
        base.SLStart();
        _inventory = ServiceLocator.Instance.GetService<InventoryController>().Inventory ??
            throw new System.NullReferenceException($"{nameof(InventoryController)} doesn't exist");
    }

    public override void HideItem()
    {
        base.HideItem();
    }

    public override void OnPickup(HandController handController)
    {
        base.OnPickup(handController);
        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>();
    }

    public override void OnThrow()
    {
        base.OnThrow();
    }

    public override void ShowItem()
    {
        base.ShowItem();
    }

    public override void StopUse()
    {
    }

    public override void Use()
    {
        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit, _currentHand.HandDistance))
        {
            if(hit.transform.TryGetComponent<Dirt>(out var dirt))
            {
                dirt.Dig(_inventory);
            }
        }

    }
}

using Assets.Scripts.Extenisions;
using Newtonsoft.Json;
using System.Linq;
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
                dirt.Dig();
                return;
            }

            var digplaces = FindObjectsByType<DigablePlace>();
            if (!digplaces.Any()) return;

            var inArea = digplaces.FirstOrDefault(x => x.CanDig(hit.point));
            var ceilPoint = hit.point.ToInt();

            var dirtController = FindAnyObjectByType<DirtController>();
            if (dirtController == null) return;

            dirtController.ShowDirt(ceilPoint.x, ceilPoint.z);

            print(hit.point);
            print(ceilPoint);
            print(inArea);
        }

    }
}

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

        OnDestroyAction += () =>
        {
            var places = FindObjectsByType<DigablePlace>();
            foreach (var place in places)
            {
                place.DisableGrid();
            }

        };
    }

    public override void HideItem()
    {
        base.HideItem();
    }

    public override void OnPickup(HandController handController)
    {
        base.OnPickup(handController);
        var places = FindObjectsByType<DigablePlace>();
        foreach(var place in places )
        {
            place.EnableGrid();
        }
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
            var digplaces = FindObjectsByType<DigablePlace>();
            if (!digplaces.Any()) return;

            var inArea = digplaces.FirstOrDefault(x => x.CanDig(hit.point));
            var ceilPoint = hit.point.ToInt();

            var dirtController = FindAnyObjectByType<DirtController>();
            if (dirtController == null) return;

            if(dirtController.ShowDirt(ceilPoint.x, ceilPoint.z))
            {
                base.Use();
                _animator?.SetTrigger("Dig");
            }
        }

    }
}

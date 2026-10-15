using UnityEngine;

public class SeedObject : ItemObject
{
    [SerializeField] private Plant _plant;
    [SerializeField] private Collider[] _colliders;
    private CameraHandler _handler;
    protected override void SLStart()
    {
        base.SLStart();
        _handler = ServiceLocator.Instance.GetService<CameraHandler>();
        _inventory = ServiceLocator.Instance.GetService<InventoryController>().Inventory ??
            throw new System.NullReferenceException($"{nameof(InventoryController)} doesn't exist");
    }

    public override void HideItem()
    {
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
    }

    public override void OnThrow()
    {
    }

    public override void ShowItem()
    {
        _rb.useGravity = false;
        _rb.isKinematic = true;

        foreach (var collider in _colliders)
        {
            collider.isTrigger = true;
        }
    }

    public override void StopUse()
    {
    }

    public override void Use()
    {
        if (Physics.Raycast(_handler.Position, _handler.GetDirection(), out var hit, _currentHand.HandDistance))
        {
            if (hit.transform.TryGetComponent<Dirt>(out var dirt))
            {
                dirt.PlacePlant(_plant, () =>
                {
                    _inventory.RemoveItemFromHand(_currentHand.CurrentHandType.ToString());
                });
            }
        }
    }
}

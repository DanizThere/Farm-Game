using UnityEngine;

public class Showel : ItemObject
{
    [SerializeField] private Collider[] _colliders;
    private CameraHandler _cameraHandler;

    protected override void SLStart()
    {
        base.SLStart();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>() ??
            throw new System.NullReferenceException($"{nameof(CameraHandler)} doesn't exist");
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
        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>();
    }

    public override void OnThrow()
    {
    }

    public override void ShowItem()
    {
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

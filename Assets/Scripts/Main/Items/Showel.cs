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
    }

    public override void HideItem()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public override void ShowItem()
    {
        throw new System.NotImplementedException();
    }

    public override void StopUse()
    {
        throw new System.NotImplementedException();
    }

    public override void Use()
    {
        if (Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit, _currentHand.HandDistance))
        {
            if(hit.transform.TryGetComponent<Plant>(out var plant))
            {
                plant.Collect(_inventory);
                return;
            }
            if(hit.transform.TryGetComponent<Dirt>(out var dirt))
            {
                dirt.Dig();
            }
        }

    }
}

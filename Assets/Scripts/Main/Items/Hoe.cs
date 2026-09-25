using UnityEngine;

public class Hoe : ItemObject
{
    [SerializeField] private Collider[] _colliders;
    private CameraHandler _handler;
    public override void InitializeInStart()
    {
        _handler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    public override void AltUse()
    {
        throw new System.NotImplementedException();
    }

    public override void HideItem()
    {

    }

    public override void OnPickup()
    {
        _rb.useGravity = false;
        _rb.isKinematic = true;
        
        foreach(var collider in _colliders)
        {
            collider.isTrigger = true;
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
    }

    public override void ShowItem()
    {

    }

    public override void StopAltUse()
    {
        throw new System.NotImplementedException();
    }

    public override void StopUse()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(_handler.Position, _handler.GetDirection(), Color.red, 100);
    }

    public override void Use()
    {
        if (Physics.Raycast(_handler.Position, _handler.GetDirection(), out var hit, 15))
        {
            print(hit.transform.gameObject);

            if (hit.transform.TryGetComponent<HoeHandler>(out var handler))
            {
                handler.TryEffort(this);
            }
        }
    }
}

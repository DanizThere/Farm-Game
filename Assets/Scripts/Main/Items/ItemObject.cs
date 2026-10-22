using UnityEngine;

public abstract class ItemObject : DefaultSelectable
{
    public Item Item => _item; 
    [SerializeField] private float _throwForce = 5f;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected Collider[] _colliders;
    [SerializeField] protected Item _item;

    protected Inventory _inventory;
    protected int _slotIndex;
    protected HandController _currentHand;
    protected GameDbContext _gameDbContext;
    protected CameraHandler _cameraHandler;

    protected override void SLStart()
    {
        base.SLStart();

        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>() ??
            throw new System.NullReferenceException();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    public virtual void ShowItem()
    {
        SetTrigger();
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }
    public virtual void HideItem()
    {
        _currentHand.ClearItem();
        _currentHand = null;

        _rb.useGravity = true;
        _rb.isKinematic = false;
    }
    public abstract void Use();
    public abstract void StopUse();
    public virtual void OnThrow()
    {
        _currentHand.ClearItem();
        _currentHand = null;

        _rb.useGravity = true;
        _rb.isKinematic = false;

        _rb.AddForce(_cameraHandler.GetForward() * _throwForce, ForceMode.Impulse);
    }
    public virtual void OnPickup(HandController handController)
    {
        _currentHand = handController;

        SetTrigger();
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }

    protected virtual void SetTrigger()
    {
        foreach(var collider in _colliders)
        {
            collider.isTrigger = true;
        }
    }

    protected virtual void UnSetTrigger()
    {
        foreach (var collider in _colliders)
        {
            collider.isTrigger = false;
        }
    }
}

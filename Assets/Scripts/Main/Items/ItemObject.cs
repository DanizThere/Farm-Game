using Assets.Scripts.Extenisions;
using System;
using UnityEngine;

public abstract class ItemObject : DefaultSelectable, ISaveableObject
{
    public System.Action OnDestroyAction = delegate { };

    public bool IsHolded => _currentHand;

    public Vector3Serialize Position => transform.position.ToSerializable();

    public QuaternionSerialize Rotation => transform.rotation.ToSerialize();

    public string TypeTag => _typeTag;
    public string InstanceId
    {
        get { return _instanceId; }
        set { _instanceId = value; }
    }
    public string ExtraData => GetExtraData();

    [SerializeField] private string _instanceId = Guid.NewGuid().ToString();

    [SerializeField] private string _typeTag;

    public Item Item => _item;


    [SerializeField] private float _throwForce = 5f;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected Collider[] _colliders;
    [SerializeField] protected Item _item;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _onHitSound;

    [SerializeField] protected Vector3 _handOffset;
    [SerializeField] protected Quaternion _handRotation = Quaternion.identity;

    protected Inventory _inventory;
    protected int _slotIndex;
    protected HandController _currentHand;
    protected GameDbContext _gameDbContext;
    protected CameraHandler _cameraHandler;

    private void OnDestroy()
    {
        OnDestroyAction?.Invoke();
    }

    protected override void SLStart()
    {
        base.SLStart();

        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>() ??
            throw new System.NullReferenceException();
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    protected virtual string GetExtraData()
    {
        return string.Empty;
    }

    public void SetPositionAndRotation(float rotation = 0)
    {
        transform.SetLocalPositionAndRotation(_handOffset, _handRotation * Quaternion.Euler(0, rotation, 0));
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
        if (_currentHand != null) return;
        _currentHand = handController;

        SetTrigger();
        _rb.useGravity = false;
        _rb.isKinematic = true;

        if (TryGetComponent<OutlineInteractor>(out var interactor))
        {
            interactor.Deny();
        }
    }

    protected virtual void LoadData(SaveObject data)
    {
        InstanceId = data.InstanceId;
        transform.position = data.Position.ToVector();
        transform.rotation = data.Rotation.Deserialize();
    }

    protected virtual void SetTrigger()
    {
        foreach(var collider in _colliders)
        {
            collider.isTrigger = true;
            collider.enabled = false;
        }
    }

    protected virtual void UnSetTrigger()
    {
        foreach (var collider in _colliders)
        {
            collider.isTrigger = false;
            collider.enabled = true;
        }
    }

    public void Load(SaveObject data)
    {
        LoadData(data);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(transform.position.sqrMagnitude > .5f)
            _audioSource.PlayOneShot(_onHitSound);  
    }
}

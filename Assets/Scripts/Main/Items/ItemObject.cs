using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : DefaultSelectable
{
    public bool IsSingleUse = false;
    public List<ItemObjectEffortData> Efforts = new(); 
    public Vector3 PositionOffset => _positionOffset;
    public Quaternion RotationOffset => _rotationOffset;

    public Observable<GameObject> CurrentModel { get; private set;}

    [SerializeField] private GameObject[] _itemModels;

    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected Animator _animator;
    protected Inventory _inventory;
    protected ToolItem _toolItem;
    protected Transform _UIParent;
    protected int _slotIndex;
    protected HandController _currentHand;
    protected GameDbContext _gameDbContext;


    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    private void Awake()
    {
        CurrentModel = new(_itemModels[0]);
    }

    protected override void SLStart()
    {
        base.SLStart();

        _gameDbContext = ServiceLocator.Instance.GetService<GameDbContext>() ??
            throw new System.NullReferenceException();
    }

    public virtual void Setup(Inventory inventory, ToolItem toolItem, Transform UIParent, int slotIndex)
    {
        _inventory = inventory;
        _toolItem = toolItem;
        _UIParent = UIParent;
        _slotIndex = slotIndex;
    }

    public abstract void ShowItem();
    public abstract void HideItem();
    public abstract void Use();
    public abstract void StopUse();
    public abstract void AltUse();
    public abstract void StopAltUse();
    public abstract void OnThrow();
    public abstract void OnPickup(HandController handController);
}

using System;
using UnityEngine;

public abstract class ItemObject : DefaultSelectable
{
    public Vector3 PositionOffset => _positionOffset;
    public Quaternion RotationOffset => _rotationOffset;

    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected Animator _animator;
    protected Inventory _inventory;
    protected ToolItem _toolItem;
    protected Transform _UIParent;
    protected int _slotIndex;


    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    public override void InitializeInStart()
    {
        base.InitializeInStart();

        foreach (var action in _actions)
        {
            if (action.ActionName == "PickupAction")
            {
                action.OnClickEvent.AddListener(OnPickup);
                action.OnClickEvent.AddListener(() => ServiceLocator.Instance.GetService<GameStateMachine>().SetState<OuterWorldGameState>());
            }
        }
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
    public abstract void OnPickup();
}

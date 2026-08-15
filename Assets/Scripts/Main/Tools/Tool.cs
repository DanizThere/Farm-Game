using System;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public Vector3 PositionOffset => _positionOffset;
    public Quaternion RotationOffset => _rotationOffset;

    [SerializeField] protected Animator _animator;
    protected Inventory _inventory;
    protected ToolItem _toolItem;
    protected Transform _UIParent;
    protected int _slotIndex;

    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    public virtual void Setup(Inventory inventory, ToolItem toolItem, Transform UIParent, int slotIndex)
    {
        _inventory = inventory;
        _toolItem = toolItem;
        _UIParent = UIParent;
        _slotIndex = slotIndex;
    }

    public abstract void Show();
    public abstract void Hide();
    public abstract void Use();
    public abstract void StopUse();
    public abstract void AltUse();
    public abstract void StopAltUse();
}

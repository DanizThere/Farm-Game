using System;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public Vector3 PositionOffset => _positionOffset;
    public Quaternion RotationOffset => _rotationOffset;

    [SerializeField] protected Animator _animator;

    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;
    public abstract void Show();
    public abstract void Hide();
    public abstract void Use();
    public abstract void StopUse();
    public abstract void AltUse();
    public abstract void StopAltUse();
}

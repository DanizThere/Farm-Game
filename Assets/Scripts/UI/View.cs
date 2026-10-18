using System;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool IsActive => _isActive;

    [SerializeField] protected Observable<bool> _isActive = new(false);

    public virtual void Show()
    {
        _isActive.Value = true;
    }
    public virtual void Hide()
    {
        _isActive.Value = false;
    }
}
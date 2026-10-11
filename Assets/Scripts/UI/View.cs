using System;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool IsActive => _isActive;

    [SerializeField] protected Observable<bool> _isActive = new(false);

    public abstract void Show();
    public abstract void Hide();
}
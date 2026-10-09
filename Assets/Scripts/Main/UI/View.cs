using System;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public int Order => _order;
    public bool IsActive => _isActive;

    [SerializeField] protected int _order;
    [SerializeField] protected Observable<bool> _isActive = new(false);

    public abstract void Show();
    public abstract void Hide();
}
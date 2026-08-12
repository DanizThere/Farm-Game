using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewController : MonoBehaviour, IService
{
    private Dictionary<Type, IView> _views = new();

    private PlayerController _playerController;

    private void Start()
    {
        _playerController = ServiceLocator.Instance.GetService<PlayerController>();

        _playerController.OnEscapeEvent += OnEscape;
    }

    private void OnEscape()
    {
        var activeAndHighOrder = _views
            .Select(x => x.Value)
            .Where(x => x.IsActive && x.Order != 0)
            .OrderByDescending(x => x.Order)
            .First();

        activeAndHighOrder.Hide();
    }

    public void Add(IView view)
    {
        _views.Add(view.GetType(), view);
    }

    public T Get<T>() where T : IView
    {
        _views.TryGetValue(typeof(T), out var view);

        return (T)view;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewController : MonoBehaviour, IService
{
    public Transform HandsTransform => _handsTransform;

    public Canvas Canvas2D => _canvas2d;
    public Canvas Canvas3D => _canvas3d;

    [SerializeField] private Canvas _canvas2d;
    [SerializeField] private Canvas _canvas3d;
    [SerializeField] private Transform _handsTransform;
    private Dictionary<Type, View> _views = new();

    public void Initialize()
    {
        var camera = ServiceLocator.Instance.GetService<CameraHandler>().Camera;
        _canvas3d.worldCamera = camera;
    }

    public void Add(View view)
    {
        _views.Add(view.GetType(), view);
    }

    public T Get<T>() where T : View
    {
        _views.TryGetValue(typeof(T), out var view);

        return (T)view;
    }
}

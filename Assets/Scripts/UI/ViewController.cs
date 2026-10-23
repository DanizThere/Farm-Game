using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewController : MonoBehaviour, IService
{
    public Transform HandsTransform => _handsTransform;
    public Transform HandsSlotTransform => _handsSlotTransform;

    public Canvas Canvas2D => _canvas2d;
    public Canvas Canvas3D => _canvas3d;

    [SerializeField] private Canvas _canvas2d;
    [SerializeField] private Canvas _canvas3d;
    [SerializeField] private Transform _handsTransform;
    [SerializeField] private Transform _handsSlotTransform;
    private Dictionary<Type, View> _views = new();

    //some part should contains in controller
    private View _currentView;

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
        if(!_views.TryGetValue(typeof(T), out var view))
        {
            throw new NullReferenceException($"{typeof(T).Name} doesn't exist");
        }

        return (T)view;
    }

    public void SetCurrentView(View view)
    {
        _currentView = view;
    }

    public void ClearCurrentView()
    {
        _currentView.Hide();
        Destroy(_currentView.gameObject);
        _currentView = null;
    }

    public View GetCurrentView() => _currentView;
}

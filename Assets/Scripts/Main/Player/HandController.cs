using System;
using UnityEngine;

[RequireComponent(typeof(InUpdateFollower))]
public class HandController : MonoBehaviour
{
    [SerializeField] private Hand _currentHand;
    [Range(.1f, 1f)]
    [SerializeField] private float _handForce = .3f;
    [SerializeField] private float _handDistance = 5f;
    private InUpdateFollower _inUpdateFollower;

    private CameraHandler _handler;
    private bool _isGrabbing;
    private PhysicObject _currentObject;
    private Observable<float> _speedScale = new(0);

    private ItemObject _currentItemObject;

    public enum Hand
    {
        Left,
        Right
    }

    public void Initialize(Transform target, CameraHandler handler, System.Action<float> onGrab)
    {
        _speedScale.AddListener(onGrab);

        _handler = handler;
        _inUpdateFollower = GetComponent<InUpdateFollower>();
        _inUpdateFollower.SetTarget(target);
        var inputHandler = ServiceLocator.Instance.GetService<InputHandler>();

        if(_currentHand == Hand.Left)
        {
            inputHandler.OnClickEvent += OnClick;
            inputHandler.OnClickHoldedEvent += OnHold;
            inputHandler.OnClickReleased += OnRelease;
        }
        else
        {
            inputHandler.OnRightClickEvent += OnClick;
            inputHandler.OnRightClickHoldedEvent += OnHold;
            inputHandler.OnRightClickReleased += OnRelease;
        }
    }

    private void OnClick()
    {
        if (Physics.Raycast(transform.position, _handler.GetDirection(), out var hiteffort, _handDistance))
        {
            if(hiteffort.transform.TryGetComponent<HoeEffortModel>(out var effort))
            {
                effort.Effort();
            }
        }

        if (_currentItemObject)
        {
            _currentItemObject.Use();
            print("use");
            return;
        }

        if(Physics.Raycast(transform.position, _handler.GetDirection(), out var hit, _handDistance)){
            if(hit.transform.TryGetComponent<ItemObject>(out _currentItemObject))
            {
                _currentItemObject.GetComponent<Collider>().enabled = false;
                _currentItemObject.transform.SetParent(transform);
                _currentItemObject.transform.rotation = Quaternion.identity;
                _currentItemObject.transform.position = transform.position;
                _currentItemObject.OnPickup();

                if(_currentItemObject is Hoe)
                {
                    var handlers = FindObjectsByType<HoeHandler>();
                    foreach(var handler in handlers)
                    {
                        handler.ShowHandle(_currentItemObject);
                    }
                }

                return;
            }

            _isGrabbing = hit.transform.TryGetComponent<PhysicObject>(out _currentObject);
            _inUpdateFollower.enabled = !_isGrabbing;

            if(_currentObject != null)
            {
                transform.position = hit.point;
                transform.SetParent(_currentObject.transform);

                _currentObject.Grab(_handForce, _handler, out var speed, _currentHand);
                _speedScale.Value = speed;
            }
        }
    }

    private void OnHold()
    {
        _currentObject?.UnGrab(_currentHand);
        //_currentItemObject?.OnThrow();

        _currentObject = null;
        _isGrabbing = false;
        _inUpdateFollower.enabled = !_isGrabbing;
        _speedScale.Value = 1;
        transform.SetParent(null);
    }

    private void OnRelease()
    {
        OnHold();
    }
}

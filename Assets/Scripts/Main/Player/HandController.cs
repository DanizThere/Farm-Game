using System;
using UnityEngine;

[RequireComponent(typeof(InUpdateFollower))]
public class HandController : MonoBehaviour
{
    public bool IsBusy => _currentItemObject;
    public bool IsTotalBusy => _currentItemObject || _effortModel || _currentObject;
    public float HandDistance => _handDistance;

    [SerializeField] private Hand _currentHand;
    [Range(.1f, 1f)]
    [SerializeField] private float _handForce = .3f;
    [SerializeField] private float _handDistance = 5f;
    private InUpdateFollower _inUpdateFollower;

    private CameraHandler _handler;
    private PhysicObject _currentObject;
    private Observable<float> _speedScale = new(0);

    private ItemObject _currentItemObject;

    private Transform _defaultTarget;
    private Transform _currentTarget;

    private EffortModel _effortModel;
    private ISelectable _selectable;

    public enum Hand
    {
        Left,
        Right
    }

    private void Update()
    {
        _effortModel?.Effort();

        if (Physics.Raycast(_handler.Position, _handler.GetDirection(), out var hit, _handDistance))
        {
            if (hit.transform.TryGetComponent<ISelectable>(out _selectable))
            {
                _selectable.Show();
                return;
            }
        }

        _selectable?.Hide();
        _selectable = null;
    }

    public void Initialize(Transform target, CameraHandler handler, System.Action<float> onGrab)
    {
        _speedScale.AddListener(onGrab);

        _handler = handler;

        _defaultTarget = target;

        _inUpdateFollower = GetComponent<InUpdateFollower>();
        _inUpdateFollower.SetTarget(_defaultTarget);
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


    public void PickupItem(ItemObject item)
    {
        _currentItemObject = item;
        _currentItemObject.OnPickup(this);
        _currentItemObject.GetComponent<Collider>().enabled = false;
        _currentItemObject.transform.SetParent(transform);
        _currentItemObject.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    public void ClearItem()
    {
        _currentItemObject = null;
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;
        _inUpdateFollower.SetTarget(_currentTarget);
    }

    public void ResetTarget()
    {
        _inUpdateFollower.SetTarget(_defaultTarget);
    }

    private void OnClick()
    {
        var raycast = Physics.Raycast(_handler.Position, _handler.GetDirection(), out var hit, _handDistance);

        if (_currentItemObject)
        {
            _currentItemObject.Use();
            return;
        }

        if(raycast)
        {
            if(TryGetItemObject(hit)) return;
            if(TryGetEffortObject(hit)) return;
            if(TryGetPhysicObject(hit)) return;

            TrySlap(hit);
        }
    }

    private void OnHold()
    {
        _speedScale.Value = 1;
        transform.SetParent(null);

        if (_currentItemObject && _currentItemObject.IsSingleUse)
        {
            _currentItemObject?.StopUse();
            _currentItemObject?.StopAltUse();
            _currentItemObject = null;

            return;
        }

        if (_currentObject)
        {
            _currentObject?.UnGrab(_currentHand);
            _currentObject = null;

            _inUpdateFollower.enabled = true;

            return;
        }

        if (_effortModel)
        {
            _effortModel = null;
        }
    }

    private void OnRelease()
    {
        OnHold();
    }

    private void TrySlap(RaycastHit hit)
    {
        if(hit.transform.TryGetComponent<ISlappable>(out var slappable))
        {
            slappable.Slap();
        }
    }


    private bool TryGetItemObject(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent<ItemObjectHandler>(out var itemObjectHandler))
        {
            var itemObject = Instantiate(itemObjectHandler.GetItemObject());
            PickupItem(itemObject);

            Destroy(itemObjectHandler.gameObject);

            return true;
        }

        return false;
    }

    private bool TryGetEffortObject(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent<EffortModel>(out _effortModel))
        {

            return true;
        }

        return false;
    }

    private bool TryGetPhysicObject(RaycastHit hit)
    {
        var isGrabbing = hit.transform.TryGetComponent<PhysicObject>(out _currentObject);
        _inUpdateFollower.enabled = !isGrabbing;

        if (_currentObject != null)
        {
            transform.position = hit.point;
            transform.SetParent(_currentObject.transform);

            _currentObject.Grab(_handForce, _handler, out var speed, _currentHand);
            _speedScale.Value = speed;
        }

        return isGrabbing;
    }
}

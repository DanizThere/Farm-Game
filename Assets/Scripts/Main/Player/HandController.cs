using System;
using UnityEngine;

[RequireComponent(typeof(InUpdateFollower))]
public class HandController : MonoBehaviour
{
    public Vector3 DefaultModelPosition {  get; private set; }
    public Transform ModelTransform => _modelTransform;
    public bool CanInteract => !IsTotalBusy && _canInteract;
    public Quaternion DefaultRotation => _defaultRotation;
    public bool IsBusy => _currentItemObject;
    public bool IsEffortOrObjectBusy => _effortModel || _currentObject;
    public bool IsTotalBusy => _currentItemObject || _effortModel || _currentObject;
    public float HandDistance => _handDistance;

    public Hand CurrentHandType => _currentHand;

    [SerializeField] private Hand _currentHand;
    [Range(.1f, 1f)]
    [SerializeField] private float _handForce = .3f;
    [SerializeField] private float _handDistance = 5f;
    [SerializeField] private Transform _modelTransform;
    [SerializeField] private float _angleClamp = 30f;
    [SerializeField] private Animator _animator;
    private InUpdateFollower _inUpdateFollower;
    private bool _canInteract;
    private bool _isHold = false;

    private CameraHandler _handler;
    private PhysicObject _currentObject;
    private Observable<float> _speedScale = new(0);

    private ItemObject _currentItemObject;

    private Transform _defaultTarget;
    private Transform _currentTarget;

    private EffortModel _effortModel;

    private Quaternion _defaultRotation;

    private void FixedUpdate()
    {
        _currentObject?.Move(_angleClamp);
    }

    public enum Hand
    {
        Left,
        Right
    }

    public void Initialize(Transform target, CameraHandler handler, System.Action<float> onGrab)
    {
        _speedScale.AddListener(onGrab);

        _defaultRotation = transform.rotation;
        DefaultModelPosition = _modelTransform.position;

        _handler = handler;

        _defaultTarget = target;

        _inUpdateFollower = GetComponent<InUpdateFollower>();
        _inUpdateFollower.SetTarget(_defaultTarget);
        var inputHandler = ServiceLocator.Instance.GetService<InputHandler>();

        var gameState = ServiceLocator.Instance.GetService<GameStateMachine>();

        gameState.GetState<OuterWorldGameState>().OnUpdateEvent += OuterUpdate;
        gameState.GetState<OuterWorldGameState>().OnStartEvent += () => SetupOuter(inputHandler);
        gameState.GetState<OuterWorldGameState>().OnExitEvent += () => DisposeOuter(inputHandler);
    }

    public bool CurrentObjectIsThat(PhysicObject obj)
    {
        return obj == _currentObject;
    }


    public void PickupItem(ItemObject item, bool fromWorld = true)
    {
        if (item == null) return;

        var go = Instantiate(item);
        go.gameObject.SetActive(true);
        _currentItemObject = go;
        _currentItemObject.OnPickup(this);
        //_currentItemObject.GetComponent<Collider>().enabled = false;
        _currentItemObject.transform.SetParent(transform);
        _currentItemObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        if(fromWorld) GetComponent<HandSlot>().SetItem(_currentItemObject.ToolItem);
    }

    public void ClearItem()
    {
        _currentItemObject = null;
        GetComponent<HandSlot>().SetItem(null);
    }

    public void DeleteItem()
    {
        if(_currentItemObject)
            Destroy(_currentItemObject.gameObject);

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
    private void OuterUpdate()
    {
        _effortModel?.Effort();

        _animator.SetBool("IsHold", _isHold && !IsTotalBusy);

        _canInteract = Physics.Raycast(_handler.Position, _handler.GetDirection(), out var hit, _handDistance)
            && hit.transform.TryGetComponent<ISelectable>(out var sel) && sel.CanSelect();
    }

    private void SetupOuter(InputHandler inputHandler)
    {
        if (_currentHand == Hand.Left)
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

    private void DisposeOuter(InputHandler inputHandler)
    {
        if (_currentHand == Hand.Left)
        {
            inputHandler.OnClickEvent -= OnClick;
            inputHandler.OnClickHoldedEvent -= OnHold;
            inputHandler.OnClickReleased -= OnRelease;
        }
        else
        {
            inputHandler.OnRightClickEvent -= OnClick;
            inputHandler.OnRightClickHoldedEvent -= OnHold;
            inputHandler.OnRightClickReleased -= OnRelease;
        }
    }

    private void OnClick()
    {
        var raycast = Physics.Raycast(_handler.Position, _handler.GetDirection(), out var hit, _handDistance);

        if (_currentItemObject)
        {
            _currentItemObject.Use();
            return;
        }

        print(_isHold && !IsTotalBusy);

        if (raycast)
        {
            if(TryGetItemObject(hit)) return;
            if(TryGetEffortObject(hit)) return;
            if(TryGetPhysicObject(hit)) return;

            TrySlap(hit);
        }

        _isHold = true;
    }

    private void OnHold()
    {
        _isHold = false;
        _speedScale.Value = 1;
        transform.SetParent(null);

        if (_currentItemObject && _currentItemObject.IsSingleUse)
        {
            _currentItemObject?.StopUse();
            _currentItemObject = null;

            return;
        }

        if (_currentItemObject)
        {
            _currentItemObject?.StopUse();
            return;
        }

        if (_currentObject)
        {
            _currentObject.UnGrab(this, out var speed);
            _speedScale.Value = speed;
            _currentObject = null;

            _inUpdateFollower.enabled = true;

            return;
        }

        if (_effortModel)
        {
            _effortModel.RemoveHand(this);
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
            _animator.SetTrigger("Slap");
            _isHold = false;
            slappable.Slap();
        }
    }


    private bool TryGetItemObject(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent<ItemObjectHandler>(out var itemObjectHandler))
        {
            PickupItem(itemObjectHandler.GetItemObject());
            _isHold = false;
            Destroy(itemObjectHandler.gameObject);
            return true;
        }

        return false;
    }

    private bool TryGetEffortObject(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent<EffortModel>(out _effortModel))
        {
            _effortModel.AddHand(this);
            _isHold = false;

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

            _currentObject.Grab(_handForce, _handler, out var speed, this);
            _speedScale.Value = speed;
            _isHold = false;
        }

        return isGrabbing;
    }
}

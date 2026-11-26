using System.Collections.Generic;
using UnityEngine;

public abstract class ToolItemObject : ItemObject
{
    public List<ItemObjectEffortData> Efforts = new();
    public Observable<GameObject> CurrentModel { get; private set; }
    [SerializeField] private GameObject[] _itemModels;
    [SerializeField] protected Animator _animator;
    [SerializeField] private bool _isDurable = true;
    private float _durability;
    private float _maxDurability = 15;

    private void Awake()
    {
        CurrentModel = new(_itemModels[0]);
        _durability = _maxDurability;
    }

    protected bool Raycast(out RaycastHit hitResult)
    {
        var result = Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit, _currentHand.HandDistance);
        hitResult = hit;
        return result;
    }

    public override void OnPickup(HandController handController)
    {
        base.OnPickup(handController);
    }

    public override void OnThrow()
    {
        base.OnThrow();
        if (TryGetComponent<OutlineInteractor>(out var interactor))
        {
            interactor.Allow();
        }
    }

    public override void Use()
    {
        if (_isDurable)
        {
            _durability--;
            if(_durability <= 0)
            {
                _currentHand?.ClearItem();
            }
        }
    }
}
